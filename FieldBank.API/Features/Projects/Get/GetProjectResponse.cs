namespace FieldBank.API.Features.Projects.Get
{
    public class GetProjectResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectTypeId { get; set; }
        public string Name { get; set; }
    }
}