using AutoMapper;
using CoreBanking.API.Models.Requests;
using CoreBanking.Application.Accounts.Commands.CreateAccount;
using CoreBanking.Application.Accounts.Commands.TransferMoney;
using CoreBanking.Application.Accounts.Queries.GetAccountDetails;
using CoreBanking.Application.Accounts.Queries.GetTransactionHistory;
using CoreBanking.Core.ValueObjects;
using Grpc.Core;
using MediatR;

namespace CoreBanking.API.gRPC.Services
{
    public class AccountGrpcService : AccountService.AccountServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountGrpcService> _logger;

        public AccountGrpcService(IMediator mediator, IMapper mapper, ILogger<AccountGrpcService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<AccountResponse> GetAccount(GetAccountRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetAccount called for {AccountNumber}", request.AccountNumber);

            // Validate input
            if (string.IsNullOrWhiteSpace(request.AccountNumber))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Account number is required"));

            try
            {
                var accountNumber = AccountNumber.Create(request.AccountNumber);
                var query = new GetAccountDetailsQuery { AccountNumber = accountNumber };
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                
                    throw new RpcException(new Status(StatusCode.NotFound,
                        string.Join("; ", result.Errors)));
                return _mapper.Map<AccountResponse>(result.Data!);


            }
            catch (Exception ex) when (ex is not RpcException)
            {
                _logger.LogError(ex, "Error retrieving account {AccountNumber}", request.AccountNumber);
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        public override async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("gRPC CreateAccount called for customer {CustomerId}", request.CustomerId);

            if (!Guid.TryParse(request.CustomerId, out var customerGuid))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format"));

            try
            {
                var command = new CreateAccountCommand
                {
                    CustomerId = CustomerId.Create(customerGuid),
                    AccountType = request.AccountType,
                    InitialDeposit = (decimal)request.InitialDeposit,
                    Currency = request.Currency
                };

                var result = await _mediator.Send(command);

                if (!result.IsSuccess)

                    throw new RpcException(new Status(StatusCode.InvalidArgument,
                        string.Join(",", result.Errors)));

                return new CreateAccountResponse
                {
                    AccountId = result.Data!.ToString(),
                    AccountNumber = "TEMP",
                    Message = "Account Created successfully"
                };
            }
            catch (Exception ex) when (ex is not RpcException)
            {
                _logger.LogError(ex, "Error creating account for customer {CustomerId}", request.CustomerId);
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }
    }
}
