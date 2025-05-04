namespace HRMS.API.Models
{
    public class JobHistoryDto
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public int? JobRoleId { get; set; }

        public string JobTitle { get; set; } = string.Empty; 
        public string EmployeeName { get; set; } = string.Empty; 
        public string ManagerName { get; set; } = string.Empty; 

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public string Comments { get; set; } = string.Empty;
    }
}
