﻿using ExpenseTracker.API.DTO.DtoModels;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.DTO.Response
{
    public class AllExpensesResponseDto
    {
        public List<ExpenseDto> Expenses { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalPages { get; set; }
    }
}
