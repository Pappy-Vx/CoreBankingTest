using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.Models;
using CoreBankingTest.Core.ValueObjects;
using CoreBankingTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _context;
        public AccountRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(AccountId accountId)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
        
        public async Task<IEnumerable<Account>> GetByCustomerIdAsync (CustomerId customerId)
        {
            return await _context.Accounts
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddAsync (Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task UpdateAsync (Account account)
        {
            _context.Accounts.Update(account);
            await Task.CompletedTask;
        }

        public async Task<bool> AccountNumberExistsAsync (AccountNumber accountNumber)
        {
            return await _context.Accounts
                .AnyAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private readonly List<AccountModel> _accounts = new()
        {
           new AccountModel { Id = 1, Name = "Alice", Balance = 1000m , Currency = CurrencyType.USD },
            new AccountModel { Id = 2, Name = "Bob", Balance = 1500m, Currency = CurrencyType.NGN },
            new AccountModel { Id = 3, Name = "Charlie", Balance = 2000m, Currency = CurrencyType.EUR}
        };

        public IEnumerable<AccountModel> GetAll() => _accounts;

        public async Task<List<Account>> GetAllAsync()
        {
            //return await _context.Accounts.Include(a => a.Transactions).ToListAsync();
            return await _context.Accounts.ToListAsync();
        }
        public AccountModel GetById(int Id) => _accounts.FirstOrDefault(a => a.Id == Id)!;
        public void Add(AccountModel account) => _accounts.Add(account);

        //public Task<Account> GetByIdAsync(Guid accountId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<Account> GetByAccountNumberAsync(AccountNumber accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }



    }
}
