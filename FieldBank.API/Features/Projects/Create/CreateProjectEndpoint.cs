using FieldBank.API.Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FieldBank.API.Features.Projects.Create
{
    public class CreateProjectEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/projects", async ([FromBody]CreateProjectRequest request, ISender sender) =>
            {
                var command = new CreateProjectCommand() {Name = request.ProjectName};

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity((object?) result.Error) 
                    : Results.Ok((object?) result.Value);
            });
        }
    }
}