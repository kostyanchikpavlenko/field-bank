using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Update
{
    public class UpdateProjectTypeCommand : IRequest<Result>
    {
        public Guid ProjectTypeId { get; set; }
        public string Name { get; set; }
    }
}