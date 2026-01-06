namespace GymManagmentDAL.Entities
{
    public class Session : BaseEntity
    {
        public string? Title { get; set; }

        public string Description { get; set; } = null!;


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Upcoming";

        #region Category- Session
        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; } = null!;
        #endregion
        #region Session - Trainer
        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;
        #endregion
        #region Session - Membersession
        public ICollection<MemberSession> SessionMember { get; set; } = null!;
        public int Capacity { get; set; }
        #endregion

    }
}
