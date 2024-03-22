using FieldBank.API.Common.Results;
using MediatR;

namespace FieldBank.API.Features.Fields.Update
{
    public class UpdateFieldCommand : IRequest<Result>
    {
        public Guid FieldId { get; set; }
        public Guid PageId { get; set; }
        public string Label { get; set; }
        public Guid InputTypeId { get; set; }
        public Guid DataTypeId { get; set; }
        public int Length { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }
        public string ValidationMessage { get; set; }
        public string InformationMessage { get; set; }
    }
}