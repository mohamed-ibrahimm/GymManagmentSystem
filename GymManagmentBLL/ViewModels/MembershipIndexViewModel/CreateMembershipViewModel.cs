using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentBLL.ViewModels.MembershipIndexViewModel
{
    public class CreateMembershipViewModel
    {
        public int MemberId { get; set; }
        public int PlaneId { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<SelectListItem>? Members { get; set; }
        public IEnumerable<SelectListItem>? Plans { get; set; }
    }
}
