using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Update
{
    public class UpdateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/project-types", 
                async (UpdateProjectTypeRequest request, ISender sender) =>
                {
                    var command = new UpdateProjectTypeCommand
                    {
                        ProjectTypeId = request.ProjectTypeId,
                        Name = request.Name
                    };

                    var result = await sender.Send(command);

                    return result.IsFailure 
                        ? Results.UnprocessableEntity(result.Error) 
                        : Results.Ok();
                });
        }
    }
}