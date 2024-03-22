using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata;

namespace FieldBank.API.Features.ProjectTypes.Delete
{
    public sealed class DeleteProjectTypeHandler(ISqlProvider sqlProvider) : IRequestHandler<DeleteProjectTypeCommand, Result>
    {
        public async Task<Result> Handle(DeleteProjectTypeCommand request, CancellationToken cancellationToken)
        {
            using var db = sqlProvider.Db;

            var query = new Query(Schema.ProjectTypes.TableName)
                .Where(Schema.ProjectTypes.ProjectTypeIdColumn, request.ProjectTypeId)
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