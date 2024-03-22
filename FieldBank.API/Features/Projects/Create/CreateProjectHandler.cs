using FieldBank.API.Common.Results;
using FieldBank.API.Common.Schemas;
using FieldBank.API.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects.Create
{
    public sealed class CreateProjectHandler
        (ISqlProvider sqlProvider, IValidator<CreateProjectCommand> validator) : IRequestHandler<CreateProjectCommand,
            Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                return Result<Guid>.UnprocessableEntity(new Error("CreateProject.FailedValidation",
                    result.ToString(";")));
            }

            using var db = sqlProvider.Db;

            var isInserted = await db.Query(Schema.Projects.TableName)
                .InsertAsync(
                    new {request.ProjectId, request.ProjectTypeId, request.Name}, cancellationToken: cancellationToken);

            if (isInserted > 0)
            {
                return Result<Guid>.Success(request.ProjectTypeId);
            }

            return Result<Guid>.UnprocessableEntity(new Error("CreateProject",
                "Project was not created due to unknown error"));
        }
    }
}