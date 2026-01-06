using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentBLL.ViewModels.MemberSessionIndexViewModel
{
    public class CreateMemberSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Sessions { get; set; } = new List<SelectListItem>();
    }
}
