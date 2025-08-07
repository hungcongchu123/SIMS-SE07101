using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Username , please")]

        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Enter Password , please")]

        public string Password { get; set; } = null!;

    }
}
