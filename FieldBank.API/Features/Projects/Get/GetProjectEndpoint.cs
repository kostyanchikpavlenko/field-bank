using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.Projects.Get
{
    public class GetProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/projects/{id}", async (Guid projectTypeId, ISender sender) =>
            {
                var query = new GetProjectQuery {ProjectTypeId = projectTypeId};
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound((object?) result.Error);
                }

                return Results.Ok((object?) result?.Value);
            }).WithOpenApi();
        }
    }
}