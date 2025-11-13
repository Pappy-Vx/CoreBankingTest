using CoreBanking.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.BackgroundJobs.Results
{
    public class InterestCalculationResult
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public decimal InterestAmount { get; private set; }
        public AccountId AccountId { get; private set; }
        public DateTime CalculationDate { get; private set; }

        // Success constructor
        private InterestCalculationResult(decimal interestAmount, AccountId accountId, DateTime calculationDate)
        {
            IsSuccess = true;
            InterestAmount = interestAmount;
            AccountId = accountId;
            CalculationDate = calculationDate;
            ErrorMessage = string.Empty;
        }

        // Failure constructor
        private InterestCalculationResult(string errorMessage, AccountId accountId, DateTime calculationDate)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            AccountId = accountId;
            CalculationDate = calculationDate;
            InterestAmount = 0;
        }

        // Static factory methods
        public static InterestCalculationResult Success(decimal interestAmount, AccountId accountId, DateTime calculationDate)
        {
            return new InterestCalculationResult(interestAmount, accountId, calculationDate);
        }

        public static InterestCalculationResult Failure(string errorMessage, AccountId accountId, DateTime calculationDate)
        {
            return new InterestCalculationResult(errorMessage, accountId, calculationDate);
        }

        // Simplified static methods (if you don't need all properties)
        public static InterestCalculationResult Success(decimal interestAmount)
        {
            return new InterestCalculationResult(interestAmount, AccountId.Create(Guid.Empty), DateTime.UtcNow);
        }

        public static InterestCalculationResult Failure(string errorMessage)
        {
            return new InterestCalculationResult(errorMessage, AccountId.Create(Guid.Empty), DateTime.UtcNow);
        }
    }
}
