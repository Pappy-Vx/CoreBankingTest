using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.ValueObjects;

namespace CoreBankingTest.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(AccountId accountId);
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByAccountNumberAsync(AccountNumber accountNumber);
        Task<IEnumerable<Account>> GetByCustomerIdAsync(CustomerId customerId);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task<bool> AccountNumberExistsAsync(AccountNumber accountNumber);
    }
}
