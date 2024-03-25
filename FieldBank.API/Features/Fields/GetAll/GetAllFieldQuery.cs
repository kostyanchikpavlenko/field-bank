using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Fields.Get
{
    public record GetAllFieldQuery : IRequest<Result<IEnumerable<GetFieldResponse>>>;
}