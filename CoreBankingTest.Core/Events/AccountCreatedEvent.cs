// CoreBanking.Core/Events/AccountCreatedEvent.cs
using CoreBankingTest.Core.Common;
using CoreBankingTest.Core.Entities;


namespace CoreBankingTest.Core.Events;

public class AccountCreatedEvent : IDomainEvent
{
    public Account Account { get; }
    public DateTime OccurredOn { get; }

    public AccountCreatedEvent(Account account)
    {
        Account = account;
        OccurredOn = DateTime.UtcNow;
    }
}