using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.Models;
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
           new AccountModel { Id = 1, Name = "Alice", Balance = 1000m , Currency = "usd" },
            new AccountModel { Id = 2, Name = "Bob", Balance = 1500m, Currency = "usd" },
            new AccountModel { Id = 3, Name = "Charlie", Balance = 2000m, Currency = "ngn" }
        };

        public IEnumerable<AccountModel> GetAll() => _accounts;
        public AccountModel GetById(int Id) => _accounts.FirstOrDefault(a => a.Id == Id)!;
        public void Add(AccountModel account) => _accounts.Add(account);


    }
}
