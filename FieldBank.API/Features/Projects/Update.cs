using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.Projects
{
    public static class Update
    {
        public class UpdateProjectRequest
        {
            public Guid ProjectId { get; set; }
            public Guid ProjectTypeId { get; set; }
            public string Name { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public Guid ProjectId { get; set; }
            public Guid ProjectTypeId { get; set; }

            public string Name { get; set; }
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

        public sealed class Handler(ISqlProvider sqlProvider, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return Result<Guid>.UnprocessableEntity(new Error("Invalid data is provided", result.ToString(";")));
                }

                using var db = sqlProvider.Db;
                
                var isUpdated = await db.Query(ProjectSchema.TableName)
                    .Where(ProjectSchema.ProjectIdColumn, request.ProjectId)
                    .UpdateAsync(new
                    {
                        Name = request.Name,
                        ProjectTypeId = request.ProjectTypeId
                    }, cancellationToken: cancellationToken);

                if (isUpdated > 0)
                {
                    return Result.Success();
                }

                return Result.UnprocessableEntity(new Error("UpdateProjectType",
                    "Project type was not updated due to unknown error"));
            }
        }
    }

    public class UpdateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/projects", 
                async (Update.UpdateProjectRequest request, ISender sender) =>
            {
                var command = new Update.Command
                {
                    ProjectId = request.ProjectId,
                    ProjectTypeId = request.ProjectTypeId,
                    Name = request.Name
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity((object?) result.Error) 
                    : Results.Ok();
            });
        }
    }
}