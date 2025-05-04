using HRMS.Core.Entities;

namespace HRMS.API.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";

        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public DepartmentDto? Department { get; set; }

        // Modify to be a list of JobHistoryDto
        public ICollection<JobHistoryDto>? JobHistories { get; set; }
    }
}
