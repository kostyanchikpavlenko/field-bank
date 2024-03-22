using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes.Create
{
    public sealed class CreateProjectTypeHandler
        (ISqlProvider sqlProvider, IValidator<CreateProjectTypeCommand> validator) : IRequestHandler<CreateProjectTypeCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateProjectTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("CreateProjectType.FailedValidation",
                    result.ToString(";")));
            }

            using var db = sqlProvider.Db;

            var isInserted = await db.Query(Schema.ProjectTypes.TableName)
                .InsertAsync(
                    new {request.ProjectTypeId, request.Name}, cancellationToken: cancellationToken);

            if (isInserted > 0)
            {
                return Result<Guid>.Success(request.ProjectTypeId);
            }

            return Result<Guid>.UnprocessableEntity(new Error("CreateProjectType",
                "Project type was not created due to unknown error"));
        }
    }
}