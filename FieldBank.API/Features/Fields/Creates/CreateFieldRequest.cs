namespace FieldBank.API.Features.Projects.Create
{
    public record CreateFieldRequest
    {
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