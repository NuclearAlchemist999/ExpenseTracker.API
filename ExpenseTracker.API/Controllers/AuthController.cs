using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.Services.AccountService;
using ExpenseTracker.API.Services.AuthService;
using ExpenseTracker.API.Services.JwtService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(
            IAccountService accountservice, 
            IAuthService authService, 
            IJwtService jwtService)
        {
            _accountservice = accountservice;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var dbUser = await _accountservice.GetAccount(request.Username);

            if (dbUser != null) return BadRequest("Username already exists.");

            var user = await _accountservice.CreateAccount(request);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var account = await _accountservice.GetAccount(request.Username);

            if (account == null) return BadRequest();

            bool isValid = await _authService.ValidateLogin(request);

            if (!isValid)
            {
                return BadRequest("Kontrollera inloggningsuppgifterna.");
            }

            var token = _jwtService.CreateToken(account.Id);

            Response.Cookies.Append("authToken", token, 
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(720)
                });

            Response.Cookies.Append("accountId", account.Id.ToString(),
                new CookieOptions
                {
                    Secure = true,
                    Path = "/",
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddMinutes(720)
                });

            return Ok();
        }
    }
}
