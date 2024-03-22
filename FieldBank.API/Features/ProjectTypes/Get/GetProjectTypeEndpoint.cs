using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Get
{
    public class GetProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/project-types/{id}", async (Guid projectTypeId, ISender sender) =>
            {
                var query = new GetProjectTypeQuery {ProjectTypeId = projectTypeId};
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result?.Value);
            }).WithOpenApi();
        }
    }
}