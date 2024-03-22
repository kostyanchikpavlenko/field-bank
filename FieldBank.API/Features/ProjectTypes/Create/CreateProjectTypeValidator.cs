using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Features.Projects;
using FieldBank.API.Features.Projects.Create;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.Create
{
    public class CreateProjectTypeValidator : AbstractValidator<CreateProjectTypeCommand>
    {
        private readonly ISqlProvider _sqlProvider;

        public CreateProjectTypeValidator(ISqlProvider sqlProvider)
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

            return await db.Query(Schema.ProjectTypes.TableName)
                .Where(Schema.ProjectTypes.ProjectTypeNameColumn, projectName)
                .CountAsync<int>(cancellationToken:token) == 0;
        }
    }
}