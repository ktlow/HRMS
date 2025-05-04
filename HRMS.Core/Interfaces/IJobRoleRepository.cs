using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces
{
    public interface IJobRoleRepository
    {
        Task<IEnumerable<JobRole>> GetAllAsync();
        Task<JobRole> GetByIdAsync(int id);
        Task AddAsync(JobRole jobrole);
        Task UpdateAsync(JobRole jobrole);
        Task DeleteAsync(JobRole jobrole);
    }
}
