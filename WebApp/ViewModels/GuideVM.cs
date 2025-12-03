using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class GuideVM
    {

        public int Id { get; set; }
        [Required]

        public string Name { get; set; } = null!;
        [Required]

        public int Age { get; set; }
    }
}
