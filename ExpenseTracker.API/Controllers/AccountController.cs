using ExpenseTracker.API.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
           _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var cookie = Request.Cookies["accountId"];

            var account = await _accountService.GetAccountById(cookie);
            
            return Ok(account);
        }
    }
}
