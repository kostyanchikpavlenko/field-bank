using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.ProjectTypes.Delete
{

        public class DeleteProjectTypeCommand : IRequest<Result>
        {
            public Guid ProjectTypeId { get; set; }
        }
}
