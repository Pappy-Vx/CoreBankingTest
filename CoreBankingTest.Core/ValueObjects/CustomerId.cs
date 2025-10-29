using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.ValueObjects
{
    public record CustomerId(Guid Value)
    {
        public static CustomerId Create() => new(Guid.NewGuid());
        public static CustomerId Create(Guid value) => new(value);
    }
}
