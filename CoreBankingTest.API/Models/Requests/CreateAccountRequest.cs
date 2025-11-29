using CoreBankingTest.Core.Enums;

namespace CoreBankingTest.API.Models.Requests;

public record CreateAccountRequest
{
    public Guid CustomerId { get; init; }
    public string AccountType { get; init; } = string.Empty;
    public decimal InitialDeposit { get; init; }
    public CurrencyType Currency { get; init; } = CurrencyType.None;
}