using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.ValueObjects
{
    public record AccountNumber
    {
        public string Value { get; }
        public AccountNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 10 )
            {
                throw new ArgumentException("Account number must be a 10-digit numeric string.");
            }
            if (!value.All(char.IsDigit))
            {
                throw new ArgumentException("Account number must contain only digits.");
            }
            Value = value;
        }
        //no casting mryhof when u need to convert to string
        public static implicit operator string(AccountNumber number) => number.Value;

        //Ensures validation happens when converting freom string to Number
        public static explicit operator AccountNumber(string value) => new (value);
        public override string ToString() => Value;
    }
}
