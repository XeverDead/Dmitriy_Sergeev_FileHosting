using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password must be confirmed")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is different")]
        public string PasswordToConfirm { get; set; }
    }
}
