using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.Projects.GetAll
{
    public class GetAllProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/projects", async (ISender sender) =>
            {
                var query = new GetAllProjectsQuery();
                var result = await sender.Send(query);

                return Results.Ok((object?)result.Value);
            }).WithOpenApi();
        }
    }
}