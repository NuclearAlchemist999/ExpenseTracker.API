﻿using ExpenseTracker.API.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
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

            if (string.IsNullOrEmpty(cookie))
            {
                return NotFound();
            }

            var accountId = Guid.Parse(cookie);

            var account = await _accountService.GetAccountById(accountId);
            
            return Ok(account);
        }
    }
}
