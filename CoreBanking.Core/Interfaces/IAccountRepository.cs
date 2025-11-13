using CoreBanking.Core.Entities;
using CoreBanking.Core.ValueObjects;

namespace CoreBanking.Core.Interfaces
{
    public interface IAccountRepository 
    {
        Task<Account> GetByIdAsync(AccountId id, CancellationToken cancellationToken = default);
        Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Account> GetByAccountNumberAsync(AccountNumber accountNumber, CancellationToken cancellationToken = default);
        //Task<IEnumerable<Account>> GetByCustomerIdAsync(CustomerId customerId);
        Task<List<Account>> GetAccountsByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
        Task AddAsync(Account entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Account entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Account entity, CancellationToken cancellationToken = default);
        Task<bool> AccountNumberExistsAsync(AccountNumber accountNumber);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        // NEW METHODS FOR BACKGROUND JOBS
        Task<List<Account>> GetInactiveAccountsSinceAsync(DateTime sinceDate, CancellationToken cancellationToken = default);
        Task<List<Account>> GetInterestBearingAccountsAsync(CancellationToken cancellationToken = default);
        Task<List<Account>> GetActiveAccountsAsync(CancellationToken cancellationToken = default);
        Task<List<Account>> GetAccountsByStatusAsync(string status, CancellationToken cancellationToken = default);
        Task<List<Account>> GetAccountsWithLowBalanceAsync(decimal minimumBalance, CancellationToken cancellationToken = default);
    }
}