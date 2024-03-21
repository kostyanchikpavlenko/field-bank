using FieldBank.API.Common.Endpoints;
using FieldBank.API.Common.Results;
using FieldBank.API.Common.SchemaDefinitions;
using FieldBank.API.Contracts;
using FieldBank.API.Database.Interfaces;
using FieldBank.API.Features.ProjectTypes;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SqlKata.Execution;

namespace FieldBank.API.Features.ProjectTypes
{
    public static class Get
    {
        public record Query : IRequest<Result<GetProjectTypeResponse>>
        {
            public Guid ProjectTypeId { get; init; }
        }

        public sealed class Handler(ISqlProvider sqlProvider) : IRequestHandler<Query, Result<GetProjectTypeResponse>>
        {
            public async Task<Result<GetProjectTypeResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var projectType = await sqlProvider.Db.Query(ProjectTypesSchema.TableName)
                    .Select(ProjectTypesSchema.ProjectTypeIdColumn, ProjectTypesSchema.ProjectTypeNameColumn)
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
            app.MapGet("api/project-types/{id}", async (Guid projectTypeId, ISender sender) =>
            {
                var query = new Get.Query {ProjectTypeId = projectTypeId};
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result?.Value);
            }).WithOpenApi();
        }
    }
}
