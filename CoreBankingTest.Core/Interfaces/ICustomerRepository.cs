using CoreBankingTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid customerId);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
        Task UpdateAsync (Customer customer);
        Task<bool> ExistsAsync(Guid customerId);
    }
}
