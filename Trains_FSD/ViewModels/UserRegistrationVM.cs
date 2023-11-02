using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Trains_FSD.ViewModels
{
    public class UserRegistrationVM
    {
        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
