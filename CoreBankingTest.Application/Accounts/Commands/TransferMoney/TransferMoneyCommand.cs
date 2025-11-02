using CoreBankingTest.Application.Common.Interfaces;
using CoreBankingTest.Application.Common.Models;
using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBankingTest.Core.Enums;

namespace CoreBanking.Application.Accounts.Commands.TransferMoney
{
    public record TransferMoneyCommand : ICommand
    {
        public string SourceAccountNumber { get; init; } = string.Empty;
        public string DestinationAccountNumber { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public CurrencyType Currency { get; init; } = CurrencyType.NGN;
        public string Reference { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }

    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferMoneyCommandHandler(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sourceAccount = await _accountRepository.GetByAccountNumberAsync(new AccountNumber(request.SourceAccountNumber));
                var destAccount = await _accountRepository.GetByAccountNumberAsync(new AccountNumber(request.DestinationAccountNumber));

                if (sourceAccount == null)
                    return Result.Failure("Source account not found");
                if (destAccount == null)
                    return Result.Failure("Destination account not found");

                sourceAccount.Transfer(
                    amount: new Money(request.Amount, request.Currency),
                    destination: destAccount,
                    reference: request.Reference,
                    description: request.Description
                    );

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch
            {

            }
            return Result.Success();
        }
    }
}