﻿using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.ParamModels;
using ExpenseTracker.API.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddExpense(CreateExpenseRequestDto request)
        {
            var cookie = Request.Cookies["accountId"];

            var expense = await _expenseService.AddExpense(request, cookie);
            
            return Ok(expense);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllExpensesByYearAndMonth([FromQuery] ExpenseParams param)
        { 
            var cookie = Request.Cookies["accountId"];

            var expenses = await _expenseService.GetAllExpensesByYearAndMonth(param, cookie);
           
            return Ok(expenses);
        }
    }
}
