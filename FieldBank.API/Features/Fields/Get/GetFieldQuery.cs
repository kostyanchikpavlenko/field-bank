using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Fields.Get
{
    public record GetFieldQuery(Guid FieldId) : IRequest<Result<GetFieldResponse>>; 
}