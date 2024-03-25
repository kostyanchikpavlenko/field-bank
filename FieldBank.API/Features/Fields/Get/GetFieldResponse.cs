namespace FieldBank.API.Features.Fields.Get
{
    public class GetFieldResponse
    {
        public Guid FieldId { get; set; } 
        public string Label { get; set; }
        public int Length { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }
        public string ValidationMessage { get; set; }
        public string InformationMessage { get; set; }
        
        public Guid InputTypeId { get; set; }
        
        public Guid DataTypeId { get; set; }
        
        public Guid PageId { get; set; }
    };
}