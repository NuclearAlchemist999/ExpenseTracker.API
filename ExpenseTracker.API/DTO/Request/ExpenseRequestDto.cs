using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTO.Request
{
    public abstract record ExpenseRequestDto
    {
        [Required(ErrorMessage = "Title is a required field.")]
        public string Title { get; init; }
        [Required(ErrorMessage = "Category is a required field.")]
        public string CategoryId { get; init; }
        [Required(ErrorMessage = "Price is a required field.")]
        public double Price { get; init; }
        [Required(ErrorMessage = "Create date is a required field.")]
        public string CreatedAt { get; init; }
    }
}
