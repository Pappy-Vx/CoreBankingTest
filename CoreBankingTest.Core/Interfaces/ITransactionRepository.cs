using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.ValueObjects;

namespace CoreBankingTest.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(TransactionId transactionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transaction>> GetByAccountIdAndDateRangeAsync(AccountId accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
