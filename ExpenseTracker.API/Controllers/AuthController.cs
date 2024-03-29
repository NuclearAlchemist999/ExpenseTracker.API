﻿using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Services.AccountService;
using ExpenseTracker.API.Services.AuthService;
using ExpenseTracker.API.Services.JwtService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IHostEnvironment _environment;

        public AuthController(
            IAccountService accountservice, 
            IAuthService authService, 
            IJwtService jwtService,
            IHostEnvironment environment)
        {
            _accountservice = accountservice;
            _authService = authService;
            _jwtService = jwtService;
            _environment = environment;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequestDto request)
        {
            var user = await _accountservice.CreateAccount(request);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var account = await _accountservice.GetAccount(request.Username);

            await _authService.ValidateLogin(request);

            var domain = _environment.IsDevelopment()
                ? null : Environment.GetEnvironmentVariable("SERVER_URL");

            var token = _jwtService.CreateToken(account.Id);

            Response.Cookies.Append("authToken", token, 
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Domain = domain,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(720)
                });

            Response.Cookies.Append("accountId", account.Id.ToString(),
                new CookieOptions
                {
                    IsEssential = true,
                    Domain = domain,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(720)
                });

            return Ok();
        }

        [Authorize]
        [HttpGet("verifyAuth")]
        public IActionResult VerifyAuth()
        {
            var response = new AuthResponseDto
            {
                IsAuth = true
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var domain = _environment.IsDevelopment()
                ? null : Environment.GetEnvironmentVariable("SERVER_URL");

            Response.Cookies.Delete("authToken",
            new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Domain = domain,
                SameSite = SameSiteMode.None,
                Secure = true,
            });

            Response.Cookies.Delete("accountId",
            new CookieOptions
            {
                Secure = true,
                Domain = domain,
                IsEssential = true,
                SameSite = SameSiteMode.None,
            });

            return Ok();
        }
    }
}
