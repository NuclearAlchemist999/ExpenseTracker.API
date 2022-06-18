using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Services.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        public AuthController(IAccountService accountservice)
        {
            _accountservice = accountservice;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var dbUser = await _accountservice.GetAccount(request.Username);

            if (dbUser != null) return BadRequest("Username already exists.");

            var user = await _accountservice.CreateAccount(request);

            return Ok(user);
        }
    }
}
