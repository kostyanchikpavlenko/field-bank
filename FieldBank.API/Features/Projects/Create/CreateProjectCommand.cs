using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Projects.Create
{
    public class CreateProjectCommand : IRequest<Result<Guid>>
    {
        public Guid ProjectId => Guid.NewGuid();
        public string Name { get; set; }
        public Guid ProjectTypeId { get; set; }
    }
}