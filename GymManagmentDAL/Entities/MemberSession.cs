
namespace GymManagmentDAL.Entities
{
    public class MemberSession : BaseEntity
    {
        //bbokingdate ==> created at
        public bool ISAttended { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public string Status { get; set; } = "Upcoming";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
