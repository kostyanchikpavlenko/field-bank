using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Contracts;
using FieldBank.API.Database.Interfaces;
using MediatR;
using SqlKata.Execution;

namespace FieldBank.API.Features.Projects
{
    public static class Get
    {
        public record Query : IRequest<Result<GetProjectResponse>>
        {
            public Guid ProjectTypeId { get; init; }
        }

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Query, Result<GetProjectResponse>>
        {
            public async Task<Result<GetProjectResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectType = await sqlProvider.Db.Query(ProjectSchema.TableName)
                    .Select(ProjectSchema.ProjectIdColumn, ProjectSchema.ProjectNameColumn,ProjectSchema.ProjectTypeIdColumn)
                    .Where(ProjectSchema.ProjectIdColumn, request.ProjectTypeId)
                    .FirstOrDefaultAsync<GetProjectResponse>(cancellationToken: cancellationToken);

                return projectType is null
                    ? Result<GetProjectResponse>.NotFound(new Error("GetProjectById",
                        "Project was not found"))
                    : Result<GetProjectResponse>.Success(projectType);
            }
        }
    }
    
    public class GetProjectTypeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/projects/{id}", async (Guid projectTypeId, ISender sender) =>
            {
                var query = new Get.Query {ProjectTypeId = projectTypeId};
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound((object?) result.Error);
                }

                return Results.Ok((object?) result?.Value);
            }).WithOpenApi();
        }
    }
}
