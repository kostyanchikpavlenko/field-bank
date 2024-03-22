using FieldBank.API.Common.Endpoints;
using FieldBank.API.Features.Projects.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FieldBank.API.Features.Fields.Creates
{
    public class CreateFieldEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/fields", async ([FromBody]CreateFieldRequest request, ISender sender) =>
            {
                var command = new CreateFieldCommand
                {
                    Label = request.Label,
                    DataTypeId = request.DataTypeId,
                    InputTypeId = request.InputTypeId,
                    PageId = request.PageId,
                    ValidationMessage = request.ValidationMessage,
                    InformationMessage = request.InformationMessage,
                    IsRequired = request.IsRequired,
                    IsReadonly = request.IsReadonly,
                    Length = request.Length
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok(result.Value);
            });
        }
    }
}