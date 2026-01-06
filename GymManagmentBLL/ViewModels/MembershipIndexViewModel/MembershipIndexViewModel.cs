namespace GymManagmentBLL.ViewModels.MembershipIndexViewModel
{
    public class MembershipIndexViewModel
    {
        public int Id { get; set; }
        public required string MemberName { get; set; }
        public required string PlanName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Status { get; set; }
    }
}
