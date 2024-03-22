using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.Update
{
    public sealed class UpdateProjectTypeHandler(ISqlProvider sqlProvider, IValidator<UpdateProjectTypeCommand> validator) : IRequestHandler<UpdateProjectTypeCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectTypeCommand request, CancellationToken cancellationToken)
        {
            ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("Invalid data is provided", result.ToString(";")));
            }

            using var db = sqlProvider.Db;
                
            var isUpdated = await db.Query(Schema.ProjectTypes.TableName)
                .Where(Schema.ProjectTypes.ProjectTypeIdColumn, request.ProjectTypeId)
                .UpdateAsync(new
                {
                    Name = request.Name
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