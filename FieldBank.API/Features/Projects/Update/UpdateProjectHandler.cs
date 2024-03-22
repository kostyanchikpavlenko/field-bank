using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Update
{
    public sealed class UpdateProjectHandler(ISqlProvider sqlProvider, IValidator<UpdateProjectCommand> validator) : IRequestHandler<UpdateProjectCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("Invalid data is provided", result.ToString(";")));
            }

            using var db = sqlProvider.Db;
                
            var isUpdated = await db.Query(Schema.Projects.TableName)
                .Where(Schema.Projects.ProjectIdColumn, request.ProjectId)
                .UpdateAsync(new
                {
                    Name = request.Name,
                    ProjectTypeId = request.ProjectTypeId
                }, cancellationToken: cancellationToken);

            if (isUpdated > 0)
            {
                return Result.Success();
            }

            return Result.UnprocessableEntity(new Error("UpdateProjectType",
                "Project type was not updated due to unknown error"));
        }
    }
}