using CoreBankingTest.Core.Models;
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
    }
}
