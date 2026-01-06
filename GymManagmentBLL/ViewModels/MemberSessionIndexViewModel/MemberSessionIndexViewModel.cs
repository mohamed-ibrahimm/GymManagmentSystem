namespace GymManagmentBLL.ViewModels.MemberSessionIndexViewModel
{
    public class MemberSessionIndexViewModel
    {
        public IEnumerable<SessionCardViewModel> Upcoming { get; set; } = new List<SessionCardViewModel>();
        public IEnumerable<SessionCardViewModel> Ongoing { get; set; } = new List<SessionCardViewModel>();
    }
}
