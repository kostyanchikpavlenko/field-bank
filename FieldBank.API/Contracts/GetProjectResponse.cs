namespace FieldBank.API.Contracts
{
    public class GetProjectResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectTypeId { get; set; }
        public string Name { get; set; }
    }
}