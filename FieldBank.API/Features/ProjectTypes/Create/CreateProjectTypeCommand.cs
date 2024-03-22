using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Create
{
    public class CreateProjectTypeCommand : IRequest<Result<Guid>>
    {
        public Guid ProjectTypeId => Guid.NewGuid();
        public string Name { get; set; }
    }
}