using CoreBankingTest.Core.Enums;

namespace CoreBankingTest.API.Models.Requests;

public record TransferMoneyRequest
{
    public string DestinationAccountNumber { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public CurrencyType Currency { get; init; } = CurrencyType.None;
    public string Reference { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}