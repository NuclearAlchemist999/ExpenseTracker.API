﻿using ExpenseTracker.API.DTO.Converters;
using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.DTO.Request;
using ExpenseTracker.API.DTO.Response;
using ExpenseTracker.API.Exceptions;
using ExpenseTracker.API.Extensions;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.ParamModels;
using ExpenseTracker.API.Repositories.ExpenseRepository;

namespace ExpenseTracker.API.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly HttpClient _httpClient;
        public ExpenseService(IExpenseRepository expenseRepo, HttpClient httpClient)
        {
            _expenseRepo = expenseRepo;
            _httpClient = httpClient;
        }

        public async Task<ExpenseDto> GetExpense(Guid expenseId)
        {
            var expense = await _expenseRepo.GetExpense(expenseId);

            if (expense is null) throw new ExpenseNotFoundException(expenseId);

            return expense.ToExpenseDto();
        }
        public async Task<ExpenseDto> AddExpense(CreateExpenseRequestDto request, string cookie)
        {
            var newExpense = new Expense
            {
                AccountId = Guid.Parse(cookie),
                Title = request.Title,
                CategoryId = Guid.Parse(request.CategoryId),
                Price = request.Price,
                CreatedAt = DateTime.Parse(request.CreatedAt).ToString("yyyy-MM-dd"),
                CreatedYear = DateTime.Parse(request.CreatedAt).Year,
                ShortMonth = DateTime.Parse(request.CreatedAt).ToString("yyyy-MM-dd").ToShortMonth()
            };

            var expense = await _expenseRepo.AddExpense(newExpense);

            return expense.ToExpenseDto();
        }

        public async Task<AllExpensesResponseDto> FilterExpenses(ExpenseParams _params,
            string cookie)
        {
            var response = await _httpClient.GetAsync("/api/startup");

            var accountId = Guid.Parse(cookie);

            string shortMonth = "";

            var totalExpenses = new List<Expense>();
            var expenses = new List<Expense>();

            var basicExp = await _expenseRepo.GetExpenses(accountId, _params.OrderBy);

            if (_params.Month == null && _params.Year == null && _params.Categories == null &&
                _params.StartDate == null && _params.EndDate == null && _params.SearchQuery == null)
            {
                shortMonth = DateTime.Now.ToString("yyyy-MM-dd").ToShortMonth();
               
                totalExpenses = basicExp.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth).ToList();
               
                expenses = basicExp.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth).Skip(Skip(_params))
                    .Take((int)_params.Limit).ToList();
            }

            if (_params.Month != null && _params.Year != null && _params.Categories != null)
            {
                shortMonth = _params.Month.ToShortMonth();

                totalExpenses = basicExp.Where(e => e.CreatedYear == _params.Year && e.ShortMonth == shortMonth &&
                    GetTitles(e, GetCategories(_params.Categories))).ToList();

                expenses = basicExp.Where(e => e.CreatedYear == _params.Year && e.ShortMonth == shortMonth &&
                    GetTitles(e, GetCategories(_params.Categories))).Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month != null && _params.Year != null && _params.Categories == null)
            {
                shortMonth = _params.Month.ToShortMonth();
               
                totalExpenses = basicExp.Where(e => e.CreatedYear == _params.Year && e.ShortMonth == shortMonth).ToList();
                
                expenses = basicExp.Where(e => e.CreatedYear == _params.Year && e.ShortMonth == shortMonth)
                    .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month != null && _params.Year == null && _params.Categories != null)
            {
                shortMonth = _params.Month.ToShortMonth();

                totalExpenses = basicExp.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth &&
                GetTitles(e, GetCategories(_params.Categories))).ToList();

                expenses = basicExp.Where(e => e.CreatedYear == DateTime.Now.Year && e.ShortMonth == shortMonth &&
                GetTitles(e, GetCategories(_params.Categories))).Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month == null && _params.Year != null && _params.Categories != null)
            {
                totalExpenses = basicExp.Where(e => e.CreatedYear == _params.Year &&
                GetTitles(e, GetCategories(_params.Categories))).ToList();

                expenses = basicExp.Where(e => e.CreatedYear == _params.Year && GetTitles(e, GetCategories(_params.Categories)))
                .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month == null && _params.Year == null && _params.Categories != null)
            {
                totalExpenses = basicExp.Where(e => GetTitles(e, GetCategories(_params.Categories))).ToList();
               
                expenses = basicExp.Where(e => GetTitles(e, GetCategories(_params.Categories)))
                    .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month == null && _params.Year != null && _params.Categories == null)
            {
                totalExpenses = basicExp.Where(e => e.CreatedYear == _params.Year).ToList();

                expenses = basicExp.Where(e => e.CreatedYear == _params.Year).Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Month != null && _params.Year == null && _params.Categories == null)
            {
                shortMonth = _params.Month.ToShortMonth();

                totalExpenses = basicExp.Where(e => e.ShortMonth == shortMonth && e.CreatedYear == DateTime.Now.Year).ToList();

                expenses = basicExp.Where(e => e.ShortMonth == shortMonth && e.CreatedYear == DateTime.Now.Year)
                    .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Categories != null && _params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = basicExp.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate),
                    DateTime.Parse(_params.EndDate), e) && GetTitles(e, GetCategories(_params.Categories))).ToList();

                expenses = basicExp.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate),
                    DateTime.Parse(_params.EndDate), e) && GetTitles(e, GetCategories(_params.Categories)))
                   .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.Categories == null && _params.StartDate != null && _params.EndDate != null)
            {
                totalExpenses = basicExp.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse
                    (_params.EndDate), e)).ToList();

                expenses = basicExp.Where(e => GetTimeInterval(DateTime.Parse(_params.StartDate), DateTime.Parse
                    (_params.EndDate), e)).Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            if (_params.SearchQuery != null)
            {
                totalExpenses = basicExp.Where(e => e.Category.Title.ToLower().Contains(_params.SearchQuery.ToLower()) ||
                e.Title.ToLower().Contains(_params.SearchQuery.ToLower()) || e.CreatedYear.ToString() == _params.SearchQuery).ToList();

                expenses = basicExp.Where(e => e.Category.Title.ToLower().Contains(_params.SearchQuery.ToLower()) ||
                e.Title.ToLower().Contains(_params.SearchQuery.ToLower()) || e.CreatedYear.ToString() == _params.SearchQuery)
                    .Skip(Skip(_params)).Take((int)_params.Limit).ToList();
            }

            double totalPages = Math.Ceiling((double)totalExpenses.Count() / (double)_params.Limit);

            var expenseValues = new AllExpensesResponseDto
            {
                Expenses = expenses.ToExpenseDtoList(),
                NumberOfExpenses = totalExpenses.Count(),
                TotalCost = (decimal)totalExpenses.Sum(e => e.Price),
                TotalPages = (int)totalPages
            };

            return expenseValues;
          
        }

        public int Skip(ExpenseParams _params)
        {
            return (int)_params.Limit * ((int)_params.Page - 1);
        }

        public string[] GetCategories(string categories)
        {
            return categories.Split(',');
        }

        public bool GetTitles(Expense expense, string[] categories)
        {
            return categories.Contains(expense.Category.Title.ToLower());
        }

        public bool GetTimeInterval(DateTime startDate, DateTime endDate, Expense expense)
        {
            return DateTime.Parse(expense.CreatedAt) >= startDate &&
            DateTime.Parse(expense.CreatedAt) <= endDate;
        }

        public async Task<bool> DeleteExpense(Guid id)
        {
            var expense = await _expenseRepo.GetExpense(id);

            if (expense is null) throw new ExpenseNotFoundException(id);

            return await _expenseRepo.DeleteExpense(id);
        }

        public async Task<ExpenseDto> UpdateExpense(Guid id, UpdateExpenseRequestDto request)
        {
            var expense = await _expenseRepo.GetExpense(id);

            if (expense is null) throw new ExpenseNotFoundException(id);

            expense.Title = request.Title;
            expense.CategoryId = Guid.Parse(request.CategoryId);
            expense.Price = request.Price;
            expense.CreatedAt = DateTime.Parse(request.CreatedAt).ToString("yyyy-MM-dd");
            expense.CreatedYear = DateTime.Parse(request.CreatedAt).Year;
            expense.ShortMonth = DateTime.Parse(request.CreatedAt).ToString("yyyy-MM-dd").ToShortMonth();
            expense.UpdatedAt = DateTime.UtcNow;

            await _expenseRepo.UpdateExpense(expense);

            return expense.ToExpenseDto(); 
        }
        public List<string> ValidateFilterParams(ExpenseParams _params)
        {
            var errors = new List<string>();

            if (_params.StartDate != null || _params.EndDate != null)
            {
                if (_params.StartDate != null && _params.EndDate == null || _params.StartDate == null &&
                    _params.EndDate != null)
                {
                    errors.Add("Both start date and end date have to be combined.");
                }
                if (DateTime.Parse(_params.StartDate) > DateTime.Parse(_params.EndDate))
                {
                    errors.Add("Start date cannot be larger than end date.");
                }
                if (_params.Year != null || _params.Month != null)
                {
                    errors.Add("Month or year cannot be combined with start date or end date.");
                }
            }
            if (_params.Limit == null || _params.Page == null)
            {
                errors.Add("Limit and page have to be entered.");
            }

            if (_params.SearchQuery != null)
            {
                if (_params.Month != null || _params.Year != null || _params.Categories != null ||
                     _params.StartDate != null || _params.EndDate != null)
                {
                    errors.Add("Search cannot be combined with other parameters.");
                }
            }

            return errors;
        }
    }
}
