using FieldBank.API.Common.Endpoints;
using FieldBank.API.Features.Fields.Get;
using MediatR;

namespace FieldBank.API.Features.Fields.GetAll
{
    public class GetAllFieldEndpoint :IEndpoint 
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/fields", async (ISender sender) =>
            {
                var query = new GetAllFieldQuery();
                
                return Results.Ok(await sender.Send(query));
            });
        }
    }
}