using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Get
{
    public record GetProjectTypeQuery : IRequest<Result<GetProjectTypeResponse>>
    {
        public Guid ProjectTypeId { get; init; }
    }
}
