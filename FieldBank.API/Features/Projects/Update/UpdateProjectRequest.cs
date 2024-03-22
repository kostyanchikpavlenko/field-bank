namespace FieldBank.API.Features.Projects.Update
{
    public class UpdateProjectRequest
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectTypeId { get; set; }
        public string ProjectName { get; set; }
    }
}