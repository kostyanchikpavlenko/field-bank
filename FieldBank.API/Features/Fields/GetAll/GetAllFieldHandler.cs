using Extensions.Web.Core.Extensions;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.Fields.Get;
using FieldBank.API.Persistence.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Fields.GetAll
{
    public class GetAllFieldHandler(ISqlProvider sqlProvider) : IRequestHandler<GetAllFieldQuery, Result<IEnumerable<GetFieldResponse>>>
    {
        public async Task<Result<IEnumerable<GetFieldResponse>>> Handle(GetAllFieldQuery request, CancellationToken cancellationToken)
        {
           using var db = sqlProvider.Db;
           
            var result = await db.Query(Schema.Fields.TableName)
                .Join("Pages", "Pages.PageId", "Fields.PageId")
                .Join("DataTypes", "DataTypes.DataTypeId", "Fields.DataTypeId")
                .Join("InputTypes", "InputTypes.InputTypeId", "Fields.InputTypeId")
                .Select(
                    "Fields.{FieldId, Label, Length, ValidationMessage, InformationMessage, IsRequired, IsReadonly}",
                    "Pages.{PageId}",
                    "DataTypes.{DataTypeId}",
                    "InputTypes.{InputTypeId}")
                .GetAsync(cancellationToken:cancellationToken);
            
            return default;
        }
    }
}