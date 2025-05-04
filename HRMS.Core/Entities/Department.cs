using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Core.Entities;

namespace HRMS.Core.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<JobRole>? JobRoles { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
