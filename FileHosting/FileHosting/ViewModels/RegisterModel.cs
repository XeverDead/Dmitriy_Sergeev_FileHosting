using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegisterModel
    {
        [StringLength(100, ErrorMessage = "Max email size is 100 symbols")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Max login size is 50 symbols")]
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [StringLength(50, ErrorMessage = "Max password size is 50 symbols")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password must be confirmed")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is different")]
        public string ConfirmPassword { get; set; }
    }
}
