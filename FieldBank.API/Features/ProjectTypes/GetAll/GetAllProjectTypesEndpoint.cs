using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.GetAll
{
    public class GetAllProjectTypesEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/project-types", async (ISender sender) =>
            {
                var query = new GetAllProjectTypesQuery();
                var result = await sender.Send(query);
                
                return Results.Ok(result.Value);
            }).WithOpenApi();
        }
    }
}