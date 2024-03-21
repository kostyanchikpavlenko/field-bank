using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Database.Interfaces;
using MediatR;
using SqlKata;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects
{
    public static class Delete
    {
        public class Command : IRequest<Result>
        {
            public Guid ProjectId { get; set; }
        }
        
        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                using var db = sqlProvider.Db;

                var isDeleted = await db.Query(ProjectSchema.TableName)
                    .Where(ProjectSchema.ProjectIdColumn, request.ProjectId)
                    .DeleteAsync(cancellationToken:cancellationToken);
                
                if (isDeleted > 0)
                {
                    return Result.Success();
                }

                return Result.UnprocessableEntity(new Error("DeleteProject",
                    "Project was not deleted due to unknown error"));
            }
        }
    }

    public class DeleteProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/projects/{id}", 
                async (Guid id, ISender sender) =>
            {
                var command = new Delete.Command
                {
                    ProjectId = id
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity((object?) result.Error) 
                    : Results.Ok();
            });
        }
    }
}