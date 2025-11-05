using CoreBanking.Application.Accounts.Queries.GetAccountSummary;
using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Application.Common.Models;
using CoreBanking.Core.Interfaces;
using MediatR;

namespace CoreBanking.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IQuery<List<CustomerDto>>;

//public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
public class  GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>

{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {

        var customers = await _customerRepository.GetAllAsync();

        var customerDtos = customers.Select(customer => new CustomerDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.PhoneNumber,
            DateRegistered = customer.DateCreated,
            IsActive = customer.IsActive,
            //Accounts = customer.Accounts.Select(account => new AccountSummaryDto
            //{
            //    AccountNumber = account.AccountNumber,
            //    AccountType = account.AccountType.ToString(),
            //    DisplayName = $"{customer.FirstName} {customer.LastName}",
            //    Balance = account.Balance.Amount,
            //    Currency = account.Balance.Currency,
            //    IsActive = account.IsActive,
            //    DateOpened = account.DateOpened
            //}).ToList()
        }).ToList();

        return Result<List<CustomerDto>>.Success(customerDtos);
    }

}