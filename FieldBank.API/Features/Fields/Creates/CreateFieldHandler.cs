using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.Fields.Creates;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Create
{
    public sealed class CreateFieldHandler
        (ISqlProvider sqlProvider, IValidator<CreateFieldCommand> validator) : IRequestHandler<CreateFieldCommand,
            Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);
            
            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("CreateField.FailedValidation",
                    result.ToString(";")));
            }

            using var db = sqlProvider.Db;

            var isInserted = await db.Query(Schema.Fields.TableName)
                .InsertAsync(
                    new
                    {
                        request.FieldId,
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

            return isInserted > 0
                ? Result<Guid>.Success(request.FieldId)
                : Result<Guid>.UnprocessableEntity(new Error("CreateField",
                    "Project was not created due to unknown error"));
        }
    }
}