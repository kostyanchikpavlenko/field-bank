using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Contracts;
using FieldBank.API.Database.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects
{
    public static class GetAll
    {
        public record Query : IRequest<Result<IEnumerable<GetProjectResponse>>> { }

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Query, Result<IEnumerable<GetProjectResponse>>> 
        {
            public async Task<Result<IEnumerable<GetProjectResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectTypes = await sqlProvider.Db.Query(ProjectSchema.TableName)
                    .Select(ProjectSchema.ProjectIdColumn, ProjectSchema.ProjectNameColumn,ProjectSchema.ProjectTypeIdColumn)
                    .GetAsync<GetProjectResponse>(cancellationToken: cancellationToken);

                return Result<IEnumerable<GetProjectResponse>>.Success(projectTypes);
            }
        }
    }
    
    public class GetAllProjectTypesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/projects", async (ISender sender) =>
            {
                var query = new GetAll.Query();
                var result = await sender.Send(query);
                
                return Results.Ok((object?) result.Value);
            }).WithOpenApi();
        }
    }
}
