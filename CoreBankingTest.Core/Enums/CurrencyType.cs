using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyType
    {
        USD = 1,
        EUR = 2,
        NGN = 3,
        GBP = 4,
        JPY = 5
    }
}
