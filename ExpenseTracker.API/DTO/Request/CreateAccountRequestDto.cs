using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTO.Request
{
    public class CreateAccountRequestDto
    {
        [Required(ErrorMessage = "Username is a required field.")]
        [MaxLength(25, ErrorMessage = "Maximum length for username is 25 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for username is 2 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is a required field.")]
        [MaxLength(25, ErrorMessage = "Maximum length for the password is 25 characters.")]
        [MinLength(8, ErrorMessage = "Minimum length for the password is 8 characters.")]
        public string Password { get; set; }
    }
}
