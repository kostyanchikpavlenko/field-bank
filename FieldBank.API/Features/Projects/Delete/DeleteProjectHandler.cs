using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Delete
{
    public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<DeleteProjectCommand, Result>
    {
        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            using var db = sqlProvider.Db;

            var isDeleted = await db.Query(Schema.Projects.TableName)
                .Where(Schema.Projects.ProjectIdColumn, request.ProjectId)
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