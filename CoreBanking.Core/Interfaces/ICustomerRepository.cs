using CoreBanking.Core.Entities;
using CoreBanking.Core.ValueObjects;

namespace CoreBanking.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(CustomerId customerId);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task UpdateAsync(Customer customer);
        Task<bool> ExistsAsync(CustomerId customerId);
    }
}