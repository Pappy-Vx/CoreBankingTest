using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.Models;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Interfaces
{
    public interface IAccountRepository
    {
        AccountModel GetById(int Id);
        IEnumerable<AccountModel> GetAll();
        void Add(AccountModel account);

        Task<Account> GetByIdAsync(Guid accountId);
        Task<Account> GetByAccountNumberAsync(AccountNumber accountNumber);
        Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task <bool> AccountNumberExistsAsync(AccountNumber accountNumber);
    }
}
