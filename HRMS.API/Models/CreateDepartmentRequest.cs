namespace HRMS.API.Models
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LocationId { get; set; }
    }
}
