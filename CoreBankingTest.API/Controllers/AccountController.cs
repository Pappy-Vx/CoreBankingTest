using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace CoreBankingTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController: ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            var accountId = AccountId.Create(id);

            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
                return NotFound($"Account with ID {id} not found.");

            return Ok(account);
        }
    }
}
