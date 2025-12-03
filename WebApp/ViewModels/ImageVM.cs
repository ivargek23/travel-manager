namespace WebApp.ViewModels
{
    public class ImageVM
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string PicturePath { get; set; } = null!;

        public string PictureName { get; set; } = null!;
    }
}
