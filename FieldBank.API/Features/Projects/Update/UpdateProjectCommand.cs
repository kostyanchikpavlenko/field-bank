using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Projects.Update
{
    public class UpdateProjectCommand : IRequest<Result>
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectTypeId { get; set; }

        public string Name { get; set; }
    }
}