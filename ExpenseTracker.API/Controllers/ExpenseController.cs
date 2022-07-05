using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.ParamModels;
using ExpenseTracker.API.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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

            return Ok(expense);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var isDeleted = await _expenseService.DeleteExpense(id);

            return isDeleted ? Ok() : StatusCode(500, "Could not delete expense.");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, UpdateExpenseRequestDto request)
        {
            var updatedExpense = await _expenseService.UpdateExpense(id, request);

            return Ok(updatedExpense);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FilterExpenses([FromQuery] ExpenseParams _params)
        {
            var cookie = Request.Cookies["accountId"];

            var errors = _expenseService.ValidateFilterParams(_params);

            if (errors.Count > 0)
            {
                var response = new ValidateFilterResponse
                {
                    Success = false,
                    Errors = errors
                };

                return BadRequest(response);
            }

            var expenses = await _expenseService.FilterExpenses(_params, cookie);
            
            return Ok(expenses);
        }
    }
}
