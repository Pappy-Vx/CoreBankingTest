using CoreBankingTest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public CurrencyType Currency { get; } = CurrencyType.NGN;

        public Money(decimal amount, CurrencyType currency = CurrencyType.NGN)
        {
            if (Amount < 0)
                throw new ArgumentOutOfRangeException("Money amount cannot be negative.");
            Amount = amount;
            Currency = currency;
        }
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add amounts with different currencies.");
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot subtract amounts with different currencies.");
            return new Money(a.Amount - b.Amount, a.Currency);
        }
    }
}
