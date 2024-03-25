using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Fields.Update
{
    public sealed class UpdateFieldHandler
        (ISqlProvider sqlProvider, IValidator<UpdateFieldCommand> validator) : IRequestHandler<UpdateFieldCommand,
            Result>
    {
        public async Task<Result> Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);
            
            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("UpdateField.FailedValidation",
                    result.ToString(";")));
            }

            using var db = sqlProvider.Db;

            var isUpdated = await db.Query(Schema.Fields.TableName)
                .Where(Schema.Fields.FieldIdColumn, request.FieldId)
                .UpdateAsync(
                    new
                    {
                        request.Label, 
                        request.PageId,
                        request.DataTypeId,
                        request.InputTypeId,
                        request.Length,
                        request.ValidationMessage,
                        request.InformationMessage,
                        request.IsRequired,
                        request.IsReadonly
                    }, cancellationToken: cancellationToken);
            return isUpdated > 0
                ? Result.Success()
                : Result.UnprocessableEntity(new Error("UpdateField",
                    "Field was not created due to unknown error"));
        }
    }
}