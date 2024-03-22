using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.Projects.Delete
{
    public class DeleteProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/projects/{id}", 
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteProjectCommand
                    {
                        ProjectId = id
                    };

                    var result = await sender.Send(command);

                    return result.IsFailure 
                        ? Results.UnprocessableEntity((object?) result.Error) 
                        : Results.Ok();
                });
        }
    }
}