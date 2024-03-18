using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Contracts;
using FieldBank.API.Database.Interfaces;
using FieldBank.API.Features.ProjectTypes;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class GetAllProjectTypes
    {
        public record Query : IRequest<Result<IEnumerable<GetProjectTypeResponse>>> { }

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Query, Result<IEnumerable<GetProjectTypeResponse>>> 
        {
            public async Task<Result<IEnumerable<GetProjectTypeResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectTypes = await sqlProvider.Db.Query("ProjectTypes")
                    .Select("ProjectTypeId", "Name")
                    .GetAsync<GetProjectTypeResponse>(cancellationToken: cancellationToken);

                return Result<IEnumerable<GetProjectTypeResponse>>.Success(projectTypes);
            }
        }
    }
    
    public class GetAllProjectTypesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/project-types", async (ISender sender) =>
            {
                var query = new GetAllProjectTypes.Query();
                var result = await sender.Send(query);
                
                return Results.Ok(result.Value);
            }).WithOpenApi();
        }
    }
}
