namespace FieldBank.API.Features.ProjectTypes.Update
{

        public class UpdateProjectTypeRequest
        {
            public Guid ProjectTypeId { get; set; }
            public string Name { get; set; }
        }
}