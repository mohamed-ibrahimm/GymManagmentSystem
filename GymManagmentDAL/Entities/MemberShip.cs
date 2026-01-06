namespace GymManagmentDAL.Entities
{
    public class MemberShip : BaseEntity
    {
        //start date => created at
        public DateTime EndDate { get; set; }

        public string Staues
        {
            get
            {
                if (EndDate < DateTime.Now)
                    return "Expired";
                return "Active";
            }
        }

        public int MemberID { get; set; }
        public Member Member { get; set; } = null!;

        public int PlaneId { get; set; }
        public Plane Plane { get; set; } = null!;

    }
}
