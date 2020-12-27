using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class LoginModel
    {
        [StringLength(100, ErrorMessage = "Max email size is 100 symbols")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Max password size is 50 symbols")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
