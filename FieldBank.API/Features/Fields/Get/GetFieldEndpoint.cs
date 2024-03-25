using FieldBank.API.Common.Endpoints;
using MediatR;

namespace FieldBank.API.Features.Fields.Get
{
    public class GetFieldEndpoint :IEndpoint 
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/fields/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetFieldQuery(id);

                var result = await sender.Send(query);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error);
            });
        }
    }
}