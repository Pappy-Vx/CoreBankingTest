using CoreBanking.Application.Common.Models;
using CoreBanking.Core.Entities;
using CoreBanking.Core.Interfaces;
using CoreBanking.Core.Enums;  // Assuming AccountType enum is here
using CoreBanking.Core.ValueObjects;  // For Money, AccountNumber, etc.
using MediatR;
using System;  // For Guid, etc.

namespace CoreBanking.Application.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : IRequest<Result<Guid>>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public DateTime DateOfBirth { get; init; }

        // Optional fields for creating an initial account
        public string? AccountType { get; init; }  // e.g., "Savings" or "Checking"
        public decimal InitialDeposit { get; init; } = 0m;  // Default to 0 if no account
        public string Currency { get; init; } = "NGN";  // Default currency
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Guid>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;  // Inject this for adding account
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Create new customer entity
            var customer = new Customer(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                address: request.Address,
                dateOfBirth: request.DateOfBirth
            );

            // Add customer to repository
            await _customerRepository.AddAsync(customer);

            // If account details provided, create and link an initial account
            if (!string.IsNullOrEmpty(request.AccountType) && request.InitialDeposit > 0)
            {
                var accountNumber = AccountNumber.Create(GenerateAccountNumber());  // Implement GenerateAccountNumber() as needed (e.g., random 10-digit string)
                var account = Account.Create(
                    customerId: customer.CustomerId,
                    accountNumber: accountNumber,
                    accountType: Enum.Parse<AccountType>(request.AccountType),
                    initialBalance: new Money(request.InitialDeposit, request.Currency)
                );

                // Link account to customer
                customer.AddAccount(account);

                // Add account to repository (assuming separate repo; if EF cascades, this may not be needed)
                await _accountRepository.AddAsync(account);
            }

            // Save changes in a single transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(customer.CustomerId.Value);
        }

        // Placeholder for account number generation (implement as per your business logic)
        private string GenerateAccountNumber()
        {
            // Example: Generate a unique 10-digit number
            return new Random().Next(1000000000, int.MaxValue).ToString();
        }

    }
}