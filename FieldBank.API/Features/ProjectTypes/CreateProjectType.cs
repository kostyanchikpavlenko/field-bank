using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class CreateProjectType
    {
        public class Command : IRequest<Result<Guid>>
        {
            public Guid ProjectTypeId => Guid.NewGuid();
            public string Name { get; set; }
        }

        private sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .MaximumLength(255).WithMessage("Length of project type name is exceeded")
                    .NotEmpty().WithMessage("Project type name is required");
            }
        }

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new Validator();

                ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return Result<Guid>.UnprocessableEntity(new Error("Invalid data is provided", result.ToString(";")));
                }

                using var db = sqlProvider.Db;

                var query = new Query("ProjectTypes")
                    .AsInsert(new {request.ProjectTypeId, request.Name});

                var isInserted = await db.ExecuteAsync(query, cancellationToken: cancellationToken);

                if (isInserted > 0)
                {
                    return Result<Guid>.Success(request.ProjectTypeId);
                }

                return Result<Guid>.UnprocessableEntity(new Error("CreateProjectType",
                    "Project type was not created due to unknown error"));
            }
        }
    }

    public class CreateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/project-types", async (string name, ISender sender) =>
            {
                var command = new CreateProjectType.Command() {Name = name};

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok(result.Value);
            });
        }
    }
}