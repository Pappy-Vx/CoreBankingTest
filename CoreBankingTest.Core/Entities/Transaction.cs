using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Entities
{
    public class Transaction
    {
        public TransactionId TransactionId { get; private set; }

        public AccountId AccountId { get; private set; }
        public Account Account { get; private set; } // Navigation Key
        public TransactionType Type { get; private set; }
        public Money Amount { get; private set; }


        public DateTime Timestamp { get; private set; }
        public string Description { get; private set; }
        public string Reference { get; private set; }

        //Required for EF Core
        private Transaction() { }

        public Transaction(AccountId accountId, Money amount, TransactionType type, string description)
        {
            TransactionId = TransactionId.Create();
            AccountId = accountId;
            Amount = amount;
            Type = type;
            Timestamp = DateTime.UtcNow;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Reference = GenerateReference();
        }

        private string GenerateReference()
        {
            return $"TXN-{Timestamp:yyyyMMddHHmmss}-{TransactionId.ToString().Substring(0,8)}";
        }

    }
}
