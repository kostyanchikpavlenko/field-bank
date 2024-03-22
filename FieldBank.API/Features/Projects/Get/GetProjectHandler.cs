using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Get
{
    public sealed class GetProjectHandler
        (ISqlProvider sqlProvider) : IRequestHandler<GetProjectQuery, Result<GetProjectResponse>>
    {
        public async Task<Result<GetProjectResponse>> Handle(GetProjectQuery request,
            CancellationToken cancellationToken)
        {
            var projectType = await sqlProvider.Db.Query(Schema.Projects.TableName)
                .Select(Schema.Projects.ProjectIdColumn, Schema.Projects.ProjectNameColumn,
                    Schema.Projects.ProjectTypeIdColumn)
                .Where(Schema.Projects.ProjectIdColumn, request.ProjectTypeId)
                .FirstOrDefaultAsync<GetProjectResponse>(cancellationToken: cancellationToken);

            return projectType is null
                ? Result<GetProjectResponse>.NotFound(new Error("GetProjectById",
                    "Project was not found"))
                : Result<GetProjectResponse>.Success(projectType);
        }
    }
}