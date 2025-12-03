namespace WebApp.ViewModels
{
    public class SearchVM
    {
        public string Term { get; set; }
        public string OrderBy { get; set; } = "";
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int FromPager { get; set; }
        public int ToPager { get; set; }
        public int LastPage { get; set; }
        public string Submit { get; set; }
        public IEnumerable<TravelVM> Travels { get; set; }  
    }
}
