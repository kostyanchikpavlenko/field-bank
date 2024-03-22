using FieldBank.API.Common.Results;
using FieldBank.API.Features.ProjectTypes.Get;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.GetAll
{
    public record GetAllProjectTypesQuery : IRequest<Result<IEnumerable<GetProjectTypeResponse>>> { }
}
