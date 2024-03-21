using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.SchemaDefinitions;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using SqlKata.Execution;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class Create
    {
        public class Command : IRequest<Result<Guid>>
        {
            public Guid ProjectTypeId => Guid.NewGuid();
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly ISqlProvider _sqlProvider;

            public Validator(ISqlProvider sqlProvider)
            {
                _sqlProvider = sqlProvider;

                RuleFor(x => x.Name)
                    .MaximumLength(255).WithMessage("Length of project type name is exceeded")
                    .NotEmpty().WithMessage("Project type name is required")
                    .MustAsync(BeUniqueProjectType)
                    .WithMessage("Project type should be unique");
            }

            private async Task<bool> BeUniqueProjectType(string type, CancellationToken token)
            {
                using var db = _sqlProvider.Db;

                return await db.Query(ProjectTypesSchema.TableName)
                    .Where(ProjectTypesSchema.ProjectTypeNameColumn, type)
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
                    return Result<Guid>.UnprocessableEntity(new Error("CreateProjectType.FailedValidation",
                        result.ToString(";")));
                }

                using var db = sqlProvider.Db;

                var isInserted = await db.Query(ProjectTypesSchema.TableName)
                    .InsertAsync(
                        new {request.ProjectTypeId, request.Name}, cancellationToken: cancellationToken);

                if (isInserted > 0)
                {
                    return Result<Guid>.Success(request.ProjectTypeId);
                }

                return Result<Guid>.UnprocessableEntity(new Error("CreateProjectType",
                    "Project type was not created due to unknown error"));
            }
        }
    }

    public class CreateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/project-types", async (string name, ISender sender) =>
            {
                var command = new Create.Command() {Name = name};

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok(result.Value);
            });
        }
    }
}