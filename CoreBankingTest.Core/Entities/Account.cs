using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreBankingTest.Core.Entities
{
    public class Account
    {
        public Guid AccountId { get; private set; }
        public AccountNumber AccountNumber { get; private set; }
        public AccountType AccountType { get; private set; }
        public Money Balance { get; private set; }
        public DateTime DateOpened { get; private set; }
        public Guid CustomerId { get; private set; }
        public bool IsActive { get; private set; }
        //Navigation Property
        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        //Required for EF Core
        private Account() { }

        public Account(AccountNumber accountNumber, Guid customerId, AccountType accountType)

        {
            AccountId = Guid.NewGuid();
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
