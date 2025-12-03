namespace WebApp.ViewModels
{
    public class UserTravelVM
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TravelId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string UserUsername { get; set; } = string.Empty;
        public string TravelName { get; set; } = string.Empty;
    }
}
