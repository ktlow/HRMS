using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HRMS.Core.Entities
{
    public class JobHistory
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        [JsonIgnore]
        public Employee? Employee { get; set; }

        public int? ManagerId { get; set; }

        [JsonIgnore]
        public Employee? Manager { get; set; }

        public int? JobRoleId { get; set; }
        public JobRole? JobRole { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active";
        public string Comments { get; set; } = string.Empty;
    }
}
