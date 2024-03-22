using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Delete
{
    public class DeleteProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/project-types/{id}", 
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteProjectTypeCommand
                    {
                        ProjectTypeId = id
                    };

                    var result = await sender.Send(command);

                    return result.IsFailure 
                        ? Results.UnprocessableEntity(result.Error) 
                        : Results.Ok();
                });
        }
    }
}