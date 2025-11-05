using CoreBanking.Application.Accounts.Queries.GetAccountSummary;
using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Application.Common.Models;
using CoreBanking.Core.Interfaces;
using CoreBanking.Core.ValueObjects;
using MediatR;

namespace CoreBanking.Application.Customers.Queries.GetCustomerDetails;

public record GetCustomerDetailsQuery : IQuery<CustomerDetailsDto>
{
    public Guid CustomerId { get; init; }
}

//public class GetCustomersDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, Result<CustomerDetailsDto>>
public class GetCustomersDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, Result<CustomerDetailsDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersDetailsQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerDetailsDto>> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {

        var customer = await _customerRepository.GetByIdAsync(CustomerId.Create(request.CustomerId));
        if (customer == null) throw new ArgumentException("Customer doesn't exist");

        var customerDtos = new CustomerDetailsDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.PhoneNumber,
            //Address = customer.Address,
            //DateOfBirth = customer.DateOfBirth,
            DateRegistered = customer.DateCreated,
            IsActive = customer.IsActive,
            Accounts = customer.Accounts.Select(account => new AccountSummaryDto
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType.ToString(),
                DisplayName = $"{customer.FirstName} {customer.LastName}",
                Balance = account.Balance.Amount,
                Currency = account.Balance.Currency,
                IsActive = account.IsActive,
                DateOpened = account.DateOpened
            }).ToList()
        };

        return Result<CustomerDetailsDto>.Success(customerDtos);
    }

}
