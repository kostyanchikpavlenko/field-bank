using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.SchemaDefinitions;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata;
using SqlKata.Execution;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class Update
    {
        public class UpdateProjectTypeRequest
        {
            public Guid ProjectTypeId { get; set; }
            public string Name { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public Guid ProjectTypeId { get; set; }
            public string Name { get; set; }
        }

        private sealed class Validator : AbstractValidator<Command>
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
                
                var isUpdated = await db.Query(ProjectTypesSchema.TableName)
                    .Where(ProjectTypesSchema.ProjectTypeIdColumn, request.ProjectTypeId)
                    .UpdateAsync(new
                    {
                        Name = request.Name
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
            app.MapPut("api/project-types", 
                async (Update.UpdateProjectTypeRequest request, ISender sender) =>
            {
                var command = new Update.Command
                {
                    ProjectTypeId = request.ProjectTypeId,
                    Name = request.Name
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok();
            });
        }
    }
}