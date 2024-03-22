using FieldBank.API.Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FieldBank.API.Features.ProjectTypes.Create
{
    public class CreateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/project-types", async ([FromBody]CreateProjectTypeRequest request, ISender sender) =>
            {
                var command = new CreateProjectTypeCommand() {Name = request.ProjectTypeName};

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok(result.Value);
            });
        }
    }
}