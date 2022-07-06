using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTO.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
