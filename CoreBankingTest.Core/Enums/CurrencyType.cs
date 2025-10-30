using System.Text.Json.Serialization;

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
