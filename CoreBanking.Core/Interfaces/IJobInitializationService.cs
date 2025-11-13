using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Interfaces
{
    public interface IJobInitializationService
    {
        Task InitializeRecurringJobsAsync();
        Task RegisterOneTimeJobsAsync();
    }
}
