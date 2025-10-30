using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.ValueObjects;

namespace CoreBankingTest.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(CustomerId customerId);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<bool> ExistsAsync(CustomerId customerId);
    }
}
