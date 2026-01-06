namespace GymManagmentBLL.ViewModels.MemberSessionIndexViewModel
{
    public class SessionCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TrainerName { get; set; } = "Unknown";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = null!;
    }
}
