using CoreBanking.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Events
{
    public class InterestCalculationCompletedEvent : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public DateTime CalculationDate { get; }
        public int SuccessfulCalculations { get; }
        public decimal TotalInterest { get; }
        public TimeSpan Duration { get; }

        public string EventType => throw new NotImplementedException();

        public InterestCalculationCompletedEvent(DateTime calculationDate, int successfulCalculations, decimal totalInterest, TimeSpan duration)
        {
            CalculationDate = calculationDate;
            SuccessfulCalculations = successfulCalculations;
            TotalInterest = totalInterest;
            Duration = duration;
        }
    }
}
