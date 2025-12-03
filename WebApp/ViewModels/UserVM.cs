using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [PasswordPropertyText(true)]
        [Required(ErrorMessage = "Confirm password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
