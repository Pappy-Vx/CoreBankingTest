using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default);
        Task AddSync(Transaction transaction, CancellationToken cancellationToken = default);

        Task<Transaction?> GetByIdAsync(TransactionId transactionId, CancellationToken cancellation = default);

        Task<IEnumerable<Transaction>> GetByAccountIdAndDateRangeAsync(AccountId accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
