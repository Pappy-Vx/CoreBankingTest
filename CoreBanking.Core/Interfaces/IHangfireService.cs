using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Interfaces
{
    public interface IHangfireService
    {
        Task<string> ScheduleJobAsync<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
        Task<string> ScheduleRecurringJobAsync<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression);
        Task<bool> DeleteJobAsync(string jobId);
        Task TriggerJobAsync<T>(Expression<Func<T, Task>> methodCall);
    }
}
