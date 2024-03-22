using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.Get
{
    public sealed class GetProjectHandler(ISqlProvider sqlProvider) : IRequestHandler<GetProjectTypeQuery, Result<GetProjectTypeResponse>>
    {
        public async Task<Result<GetProjectTypeResponse>> Handle(GetProjectTypeQuery request, CancellationToken cancellationToken)
        {
            var projectType = await sqlProvider.Db.Query(Schema.ProjectTypes.TableName)
                .Select(Schema.ProjectTypes.ProjectTypeIdColumn, Schema.ProjectTypes.ProjectTypeNameColumn)
                .Where("ProjectTypeId", request.ProjectTypeId)
                .FirstOrDefaultAsync<GetProjectTypeResponse>(cancellationToken: cancellationToken);

            return projectType is null
                ? Result<GetProjectTypeResponse>.NotFound(new Error("GetProjectTypeById",
                    "Project type was not found"))
                : Result<GetProjectTypeResponse>.Success(projectType);
        }
    }
}