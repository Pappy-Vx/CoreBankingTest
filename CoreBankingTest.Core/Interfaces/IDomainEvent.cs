// CoreBanking.Core/Common/IDomainEvent.cs
namespace CoreBankingTest.Core.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}