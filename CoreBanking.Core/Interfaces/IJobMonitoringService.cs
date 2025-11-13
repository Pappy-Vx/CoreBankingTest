using CoreBanking.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Interfaces
{
    public interface IJobMonitoringService
    {
        Task<JobStatistics> GetJobStatisticsAsync();
        Task<List<FailedJob>> GetRecentFailedJobsAsync(int count = 50);
        Task<bool> RetryFailedJobAsync(string jobId);
    }
}
