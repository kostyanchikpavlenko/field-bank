using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.Update
{
    public class UpdateProjectTypeValidator : AbstractValidator<UpdateProjectTypeCommand>
    {
        private readonly ISqlProvider _sqlProvider;

        public UpdateProjectTypeValidator(ISqlProvider sqlProvider)
        {
            _sqlProvider = sqlProvider;
                
            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Length of project type name is exceeded")
                .NotEmpty().WithMessage("Project type name is required")
                .MustAsync(BeUniqueProjectType)
                .WithMessage("Project type should be unique");
        }

        private async Task<bool> BeUniqueProjectType(string type, CancellationToken token)
        {
            using var db = _sqlProvider.Db;

            return await db.Query(Schema.ProjectTypes.TableName)
                .Where(Schema.ProjectTypes.ProjectTypeNameColumn, type)
                .CountAsync<int>(cancellationToken:token) == 0;
        }
    }
}