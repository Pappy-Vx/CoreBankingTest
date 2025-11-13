using CoreBanking.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.BackgroundJobs
{
    public interface IAccountMaintenanceService
    {
        Task<AccountMaintenanceResult> CleanupInactiveAccountsAsync(CancellationToken cancellationToken = default);
        Task<AccountMaintenanceResult> ArchiveOldTransactionsAsync(DateTime cutoffDate, CancellationToken cancellationToken = default);
        Task<AccountMaintenanceResult> ValidateAccountDataAsync(CancellationToken cancellationToken = default);
        Task<AccountMaintenanceResult> UpdateAccountStatusesAsync(CancellationToken cancellationToken = default);
    }

}
