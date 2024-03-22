using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.ProjectTypes.Get;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.GetAll
{
    public sealed class GetAllProjectTypesHandler(ISqlProvider sqlProvider) : IRequestHandler<GetAllProjectTypesQuery, Result<IEnumerable<GetProjectTypeResponse>>> 
    {
        public async Task<Result<IEnumerable<GetProjectTypeResponse>>> Handle(GetAllProjectTypesQuery request, CancellationToken cancellationToken)
        {
            var projectTypes = await sqlProvider.Db.Query(Schema.ProjectTypes.TableName)
                .Select(Schema.ProjectTypes.ProjectTypeIdColumn, Schema.ProjectTypes.ProjectTypeNameColumn)
                .GetAsync<GetProjectTypeResponse>(cancellationToken: cancellationToken);

            return Result<IEnumerable<GetProjectTypeResponse>>.Success(projectTypes);
        }
    }
}