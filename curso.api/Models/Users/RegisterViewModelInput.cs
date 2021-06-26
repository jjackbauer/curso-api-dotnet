using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Users
{
    public class RegisterViewModelInput
    {
        [Required(ErrorMessage = "Login is mandatory")]
        public string Login { get; set; }
        [Required(ErrorMessage = "E-mail is mandatory")]
        public string Email { get; set;}
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
    }
}
