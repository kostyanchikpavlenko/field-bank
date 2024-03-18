using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Database.Interfaces;
using FluentValidation;
using MediatR;
using SqlKata;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class UpdateProjectType
    {
        public class UpdateProjectTypeRequest
        {
            public Guid ProjectTypeId { get; set; }
            public string Name { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public Guid ProjectTypeId { get; set; }
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

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new Validator();

                ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                {
                    return Result<Guid>.UnprocessableEntity(new Error("Invalid data is provided", result.ToString(";")));
                }

                using var db = sqlProvider.Db;

                var query = new Query("ProjectTypes")
                    .Where("ProjectTypeId", request.ProjectTypeId)
                    .AsUpdate(new
                    {
                       Name = request.Name
                    });

                var isUpdated = await db.ExecuteAsync(query, cancellationToken: cancellationToken);

                if (isUpdated > 0)
                {
                    return Result.Success();
                }

                return Result.UnprocessableEntity(new Error("UpdateProjectType",
                    "Project type was not created due to unknown error"));
            }
        }
    }

    public class UpdateProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("api/project-types", 
                async (UpdateProjectType.UpdateProjectTypeRequest request, ISender sender) =>
            {
                var command = new UpdateProjectType.Command
                {
                    ProjectTypeId = request.ProjectTypeId,
                    Name = request.Name
                };

                var result = await sender.Send(command);

                return result.IsFailure 
                    ? Results.UnprocessableEntity(result.Error) 
                    : Results.Ok();
            });
        }
    }
}