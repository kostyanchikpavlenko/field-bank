using Extensions.Web.Core.Extensions;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using SqlKata.Execution;

namespace FieldBank.API.Features.Fields.Get
{
    public class GetFieldHandler(ISqlProvider sqlProvider) : IRequestHandler<GetFieldQuery, Result<GetFieldResponse>>
    {
        public async Task<Result<GetFieldResponse>> Handle(GetFieldQuery request, CancellationToken cancellationToken)
        {
           using var db = sqlProvider.Db;
           
            var result = await db.Query(Schema.Fields.TableName)
                .Join("Pages", "Pages.PageId", "Fields.PageId")
                .Join("DataTypes", "DataTypes.DataTypeId", "Fields.DataTypeId")
                .Join("InputTypes", "InputTypes.InputTypeId", "Fields.InputTypeId")
                .Select(
                    "Fields.{FieldId, Label, Length, ValidationMessage, InformationMessage, IsRequired, IsReadonly}",
                    "Pages.{PageId,Name}",
                    "DataTypes.{DataTypeId, Type}",
                    "InputTypes.{InputTypeId, Type}")
                .Where(Schema.Fields.FieldIdColumn, request.FieldId)
                .FirstOrDefaultAsync(cancellationToken:cancellationToken) as IDictionary<string, object>;
            
            return result is null 
                ?
                 Result<GetFieldResponse>.NotFound(new Error("GetFieldById",
                    $"Field with id {request.FieldId} was not found"))
            : new GetFieldResponse
            {
                FieldId = result["FieldId"].ToGuid(),
                Label = result["Label"].ToStringSafe(),
                Length = result["Length"].ToInt32(),
                ValidationMessage = result["ValidationMessage"].ToStringSafe(),
                InformationMessage = result["InformationMessage"].ToStringSafe(),
                IsRequired = result["IsRequired"].ToBoolean(),
                IsReadonly = result["IsReadonly"].ToBoolean(),
                DataTypeId = result["DataTypeId"].ToGuid(),
                InputTypeId = result["InputTypeId"].ToGuid(),
                PageId = result["PageId"].ToGuid()
            };
        }
    }
}