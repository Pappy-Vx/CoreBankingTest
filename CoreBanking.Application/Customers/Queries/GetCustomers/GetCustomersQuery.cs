using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Application.Common.Models;
using CoreBanking.Core.Interfaces;
using MediatR;

namespace CoreBanking.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IQuery<List<CustomerDto>>;

//public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>

public class GetCustomerQuery : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly ICustomerRepository _customerRepository;
    public GetCustomerQuery(ICustomerRepository customerRepository)
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
            Address = customer.Address,
            Phone = customer.PhoneNumber,
            DateRegistered = customer.DateCreated,
            IsActive = customer.IsActive
        }).ToList();

        return Result<List<CustomerDto>>.Success(customerDtos);
    }
}



