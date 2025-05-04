namespace HRMS.API.Models
{
    public class CreateJobHistoryRequest
    {
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int? ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Active";
        public string Comments { get; set; } = string.Empty;
    }
}
