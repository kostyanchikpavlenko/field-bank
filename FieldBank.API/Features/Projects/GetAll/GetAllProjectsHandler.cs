using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.Projects.Get;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.GetAll
{
    public sealed class GetAllProjectsHandler(ISqlProvider sqlProvider) : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<GetProjectResponse>>> 
    {
        public async Task<Result<IEnumerable<GetProjectResponse>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectTypes = await sqlProvider.Db.Query(Schema.Projects.TableName)
                .Select(Schema.Projects.ProjectIdColumn, Schema.Projects.ProjectNameColumn,Schema.Projects.ProjectTypeIdColumn)
                .GetAsync<GetProjectResponse>(cancellationToken: cancellationToken);

            return Result<IEnumerable<GetProjectResponse>>.Success(projectTypes);
        }
    }
}