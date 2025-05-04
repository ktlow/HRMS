using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces
{
    public interface IJobHistoryRepository
    {
        Task<IEnumerable<JobHistory>> GetAllAsync();
        Task<JobHistory> GetByIdAsync(int id);
        Task AddAsync(JobHistory jobhistory);
        Task UpdateAsync(JobHistory jobhistory);
        Task DeleteAsync(JobHistory jobhistory);
        Task<IEnumerable<JobHistory>> GetByEmployeeIdAsync(int employeeId);

        Task<JobHistory> AddJobHistory(int employeeId, JobHistory jobHistory);
    }
}
