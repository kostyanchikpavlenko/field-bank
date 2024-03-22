using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Projects.Get
{
    public record GetProjectQuery : IRequest<Result<GetProjectResponse>>
    {
        public Guid ProjectTypeId { get; init; }
    }
}