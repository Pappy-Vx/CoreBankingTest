using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.Models;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<AccountModel> _accounts = new()
        {
           new AccountModel { Id = 1, Name = "Alice", Balance = 1000m , Currency = CurrencyType.USD },
            new AccountModel { Id = 2, Name = "Bob", Balance = 1500m, Currency = CurrencyType.NGN },
            new AccountModel { Id = 3, Name = "Charlie", Balance = 2000m, Currency = CurrencyType.EUR}
        };

        public IEnumerable<AccountModel> GetAll() => _accounts;
        public AccountModel GetById(int Id) => _accounts.FirstOrDefault(a => a.Id == Id)!;
        public void Add(AccountModel account) => _accounts.Add(account);

        public Task<Account> GetByIdAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetByAccountNumberAsync(AccountNumber accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AccountNumberExistsAsync(AccountNumber accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
