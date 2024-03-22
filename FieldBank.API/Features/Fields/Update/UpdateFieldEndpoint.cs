using FieldBank.API.Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FieldBank.API.Features.Fields.Update
{
    public class UpdateFieldEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/fields", async ([FromBody]UpdateFieldRequest request, ISender sender) =>
            {
                var command = new UpdateFieldCommand
                {
                    FieldId = request.FieldId,
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
                    : Results.Ok();
            });
        }
    }
}