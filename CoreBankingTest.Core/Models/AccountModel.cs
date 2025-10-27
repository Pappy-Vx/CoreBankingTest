using CoreBankingTest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public CurrencyType Currency { get; set; }

    }
}
