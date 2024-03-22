using FieldBank.API.Common.Results;
using FieldBank.API.Features.Projects.Get;
using MediatR;

namespace FieldBank.API.Features.Projects.GetAll
{
    public record GetAllProjectsQuery : IRequest<Result<IEnumerable<GetProjectResponse>>> { }
}