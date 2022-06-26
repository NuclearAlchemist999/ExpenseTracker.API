using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.Request;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(Guid id)
        {
            var expense = await _expenseService.GetExpense(id);

            if (expense == null) return NotFound(); 

            return Ok(expense.ToExpenseDto());
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _expenseService.GetExpense(id);

            if (expense == null) return NotFound();

            var isDeleted = await _expenseService.DeleteExpense(id);

            return isDeleted ? Ok() : StatusCode(500, "Could not delete expense.");
        }
    }
}
