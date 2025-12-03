namespace WebAPI.Dtos
{
    public class SearchDto
    {
        public string Term { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
