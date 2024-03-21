using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.SchemaDefinitions;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class Delete
    {
        public class Command : IRequest<Result>
        {
            public Guid ProjectTypeId { get; set; }
        }
        
        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                using var db = sqlProvider.Db;

                var query = new Query(ProjectTypesSchema.TableName)
                    .Where(ProjectTypesSchema.ProjectTypeIdColumn, request.ProjectTypeId)
                    .AsDelete();

                var isDeleted = await db.ExecuteAsync(query, cancellationToken: cancellationToken);

                if (isDeleted > 0)
                {
                    return Result.Success();
                }

                return Result.UnprocessableEntity(new Error("DeleteProjectType",
                    "Project type was not deleted due to unknown error"));
            }
        }
    }

    public class DeleteProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/project-types/{id}", 
                async (Guid id, ISender sender) =>
            {
                var command = new Delete.Command
                {
                    ProjectTypeId = id
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok();
            });
        }
    }
}