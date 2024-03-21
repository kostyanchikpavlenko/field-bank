using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects
{
    public static class Create
    {
        public class Command : IRequest<Result<Guid>>
        {
            public Guid ProjectId => Guid.NewGuid();
            public string Name { get; set; }
            public Guid ProjectTypeId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly ISqlProvider _sqlProvider;

            public Validator(ISqlProvider sqlProvider)
            {
                _sqlProvider = sqlProvider;

                RuleFor(x => x.Name)
                    .MaximumLength(255).WithMessage("Length of project name is exceeded")
                    .NotEmpty().WithMessage("Project type is required")
                    .MustAsync(BeUniqueProject)
                    .WithMessage("Project type should be unique");
            }

            private async Task<bool> BeUniqueProject(string projectName, CancellationToken token)
            {
                using var db = _sqlProvider.Db;

                return await db.Query(ProjectSchema.TableName)
                    .Where(ProjectSchema.ProjectNameColumn, projectName)
                    .CountAsync<int>(cancellationToken:token) == 0;
            }
        }

        public sealed class Handler
            (ISqlProvider sqlProvider, IValidator<Command> validator) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return Result<Guid>.UnprocessableEntity(new Error("CreateProject.FailedValidation",
                        result.ToString(";")));
                }

                using var db = sqlProvider.Db;

                var isInserted = await db.Query(ProjectSchema.TableName)
                    .InsertAsync(
                        new
                        {
                            request.ProjectId, 
                            request.ProjectTypeId, 
                            request.Name
                        }, cancellationToken: cancellationToken);

                if (isInserted > 0)
                {
                    return Result<Guid>.Success(request.ProjectTypeId);
                }

                return Result<Guid>.UnprocessableEntity(new Error("CreateProject",
                    "Project was not created due to unknown error"));
            }
        }
    }

    public class CreateProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/projects", async (string name, ISender sender) =>
            {
                var command = new Create.Command() {Name = name};

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity((object?) result.Error) 
                    : Results.Ok((object?) result.Value);
            });
        }
    }
}