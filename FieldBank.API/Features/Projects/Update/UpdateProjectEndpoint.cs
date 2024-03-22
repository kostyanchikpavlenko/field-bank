using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.Projects.Update
{
    public class UpdateProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/projects", 
                async (UpdateProjectRequest request, ISender sender) =>
                {
                    var command = new UpdateProjectCommand
                    {
                        ProjectId = request.ProjectId,
                        ProjectTypeId = request.ProjectTypeId,
                        Name = request.ProjectName
                    };

                    var result = await sender.Send(command);

                    return result.IsFailure 
                        ? Results.UnprocessableEntity((object?) result.Error) 
                        : Results.Ok();
                });
        }
    }
}