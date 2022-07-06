using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTO.Request
{
    public abstract record ExpenseRequestDto
    {
        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the title is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the title is 2 characters.")]
        public string Title { get; init; }
        [Required(ErrorMessage = "Category is a required field.")]
        public string CategoryId { get; init; }
        [Range(1, double.MaxValue, ErrorMessage = "Price is required and it can't be lower than 1")]
        public double Price { get; init; }
        [Required(ErrorMessage = "Create date is a required field.")]
        public string CreatedAt { get; init; }
    }
}
