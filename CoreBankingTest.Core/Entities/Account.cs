using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreBankingTest.Core.Entities
{
    public class Account: ISoftDelete
    {
        public AccountId AccountId { get; private set; }
        public Customer Customer { get; private set; } // Navigation Key
        public AccountNumber AccountNumber { get; private set; }
        public AccountType AccountType { get; private set; }
        public Money Balance { get; private set; }
        public DateTime DateOpened { get; private set; }
        public CustomerId CustomerId { get; private set; }
        public bool IsActive { get; private set; }

        // the Account entity implements the interface and creates its on business logic for soft deletions
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }

        public void SoftDelete(string deletedBy)
        {
            if (Balance.Amount != 0)
                throw new InvalidOperationException("Cannot close account with non-zero balance");

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        //Navigation Property
        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        //Required for EF Core
        private Account() { }

        public Account(AccountNumber accountNumber, Guid customerId, AccountType accountType)

        {
            AccountId = AccountId.Create();
            AccountNumber = accountNumber;
            AccountType = accountType;
            Balance = new Money(0, CurrencyType.NGN);
            DateOpened = DateTime.UtcNow;
            IsActive = true;
        }

        public Transaction Deposit(Money amount, string description = "Deposit")
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot deposit to an inactive account.");
            if (amount.Amount <= 0)
                throw new ArgumentOutOfRangeException("Deposit amount must be positive.");

            Balance += amount;

            var transaction = new Transaction(accountId: AccountId, amount: amount, type: TransactionType.Deposit, description: description);

            _transactions.Add(transaction);
            return transaction;
        }

        //Core Banking Operations - this are aggregrates for public API
        public Transaction Withdrawal (Money amount, string description = "Withdrawal")
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot withdraw from an inactive account.");
            if (amount.Amount <= 0)
                throw new ArgumentOutOfRangeException("Withdrawal amount must be positive.");
            if (Balance.Amount < amount.Amount)
                throw new InvalidOperationException("Insufficient funds for withdrawal.");

            //special business rule for savings accounts
            if (AccountType == AccountType.Savings && _transactions.Count(t => t.Type == TransactionType.Withdrawal) >= 6)
                throw new InvalidOperationException("Savings account withdrawal limit reached.");

            Balance -= amount;
            var transaction = new Transaction(accountId: AccountId, amount: amount, type: TransactionType.Withdrawal, description: description);
            _transactions.Add(transaction);
            return transaction;
        }

        public void CloseAccount()
        {
            if (Balance.Amount != 0)
                throw new InvalidOperationException("Cannot close account with remaining balance.");
            IsActive = false;
        }
    }
}
