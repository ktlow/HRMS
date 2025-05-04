using HRMS.Core.Entities;

namespace HRMS.API.Models
{
    public class JobRoleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public DepartmentDto? Department { get; set; }
    }
}
