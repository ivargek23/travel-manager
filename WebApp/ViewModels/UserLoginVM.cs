using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class UserLoginVM
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [MinLength(6)]

        public string Password { get; set; } = null!;
        public int Role { get; set; }
        public string ReturnUrl { get; set; }
    }
}
