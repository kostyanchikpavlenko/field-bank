namespace FieldBank.API.Features.ProjectTypes.Create
{
    public record CreateProjectTypeRequest
    {
        public string ProjectTypeName { get; set; }
    };
}