using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Projects.Delete
{
    public class DeleteProjectCommand : IRequest<Result>
        {
            public Guid ProjectId { get; set; }
        }
    }
