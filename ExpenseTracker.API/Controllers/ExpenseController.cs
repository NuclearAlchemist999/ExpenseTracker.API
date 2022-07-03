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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _expenseService.GetExpense(id);

            if (expense == null) return NotFound();

            var isDeleted = await _expenseService.DeleteExpense(id);

            return isDeleted ? Ok() : StatusCode(500, "Could not delete expense.");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, UpdateExpenseRequestDto request)
        {
            var expense = await _expenseService.GetExpense(id);

            if (expense == null) return NotFound();

            var updatedExpense = await _expenseService.UpdateExpense(id, request);

            return Ok(updatedExpense.ToExpenseDto());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FilterExpenses([FromQuery] ExpenseParams _params)
        {
            var cookie = Request.Cookies["accountId"];

            if (_params.StartDate != null || _params.EndDate != null)
            {
                if (_params.StartDate != null && _params.EndDate == null || _params.StartDate == null &&
                    _params.EndDate != null)
                {
                    return BadRequest("Both start date and end date have to be combined.");
                }
                if (DateTime.Parse(_params.StartDate) > DateTime.Parse(_params.EndDate))
                {
                    return BadRequest("Start date cannot be larger than end date.");
                }
                if (_params.Year != null || _params.Month != null)
                {
                    return BadRequest("Month or year cannot be combined with start date or end date.");
                }              
            }
            if (_params.Limit == null || _params.Page == null)
            {
                return BadRequest("Limit and page have to be entered.");
            }

            var expenses = await _expenseService.FilterExpenses(_params, cookie);
            
            return Ok(expenses);
        }


        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test([FromQuery] ExpenseParams _params)
        {
            return Ok();
        }
    }
}
