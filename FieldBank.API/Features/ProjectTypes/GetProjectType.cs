using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Contracts;
using FieldBank.API.Database.Interfaces;
using FieldBank.API.Features.ProjectTypes;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class GetProjectType
    {
        public record Query : IRequest<Result<GetProjectTypeResponse>>
        {
            public Guid ProjectTypeId { get; init; }
        }

        public sealed class Handler : IRequestHandler<Query, Result<GetProjectTypeResponse>>
        {
            private readonly ISqlProvider _sqlProvider;

            public Handler(ISqlProvider sqlProvider)
            {
                _sqlProvider = sqlProvider;
            }

            public async Task<Result<GetProjectTypeResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectType = await _sqlProvider.Db.Query("ProjectTypes")
                    .Select("ProjectTypeId", "Name")
                    .Where("ProjectTypeId", request.ProjectTypeId)
                    .FirstOrDefaultAsync<GetProjectTypeResponse>(cancellationToken: cancellationToken);

                return projectType is null
                    ? Result<GetProjectTypeResponse>.NotFound(new Error("GetProjectTypeById",
                        "Project type was not found"))
                    : Result<GetProjectTypeResponse>.Success(projectType);
            }
        }
    }
    
    public class GetProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/project-type/{id}", async (Guid projectTypeId, ISender sender) =>
            {
                var query = new GetProjectType.Query {ProjectTypeId = projectTypeId};
                var result = await sender.Send(query);

                if (result?.IsFailure == true)
                {
                    return Result<GetProjectTypeResponse>.NotFound(result.Error);
                }

                return Result<GetProjectTypeResponse>.Success(result?.Value);
            }).WithOpenApi();
        }
    }
}
