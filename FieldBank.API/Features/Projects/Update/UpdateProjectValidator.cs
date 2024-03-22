using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Update
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly ISqlProvider _sqlProvider;

        public UpdateProjectValidator(ISqlProvider sqlProvider)
        {
            _sqlProvider = sqlProvider;

            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Length of project name is exceeded")
                .NotEmpty().WithMessage("Project type is required")
                .MustAsync(BeUniqueProject)
                .WithMessage("Project type should be unique");
        }

        private async Task<bool> BeUniqueProject(string projectName, CancellationToken token)
        {
            using var db = _sqlProvider.Db;

            return await db.Query(Schema.Projects.TableName)
                .Where(Schema.Projects.ProjectNameColumn, projectName)
                .CountAsync<int>(cancellationToken:token) == 0;
        }
    }
}