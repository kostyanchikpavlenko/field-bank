using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.Fields.Creates;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using SqlKata.Execution;

namespace FieldBank.API.Features.Fields.Update
{
    public class UpdateFieldValidator : AbstractValidator<UpdateFieldCommand>
    {
        private readonly ISqlProvider _sqlProvider;

        public UpdateFieldValidator(ISqlProvider sqlProvider)
        {
            _sqlProvider = sqlProvider;
            
            RuleFor(x => x.Label)
                .MaximumLength(255).WithMessage("Length of label is exceeded")
                .NotEmpty().WithMessage("Label is required")
                .WithMessage("Label should be unique in page");

            RuleFor(x => x.PageId)
                .NotEmpty().WithMessage("PageId should not be empty")
                .MustAsync(ExistPage).WithMessage("Field can be created for existing page only");

            RuleFor(x => x.DataTypeId)
                .NotEmpty().WithMessage("DataTypeId should not be empty");

            RuleFor(x => x.InputTypeId)
                .NotEmpty().WithMessage("InputTypeId should not be empty");
        }

        private async Task<bool> ExistPage(Guid pageId, CancellationToken token)
        {
            using var db = _sqlProvider.Db;

            return await db.Query(Schema.Pages.TableName)
                .Select(Schema.Pages.PageIdColumn)
                .Where(Schema.Pages.PageIdColumn, pageId)
                .FirstOrDefaultAsync<Guid>(cancellationToken:token) != Guid.Empty;
        }
    }
}