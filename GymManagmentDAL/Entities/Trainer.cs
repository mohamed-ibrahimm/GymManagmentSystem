using GymManagmentDAL.Entities.Enums;

namespace GymManagmentDAL.Entities
{
    public class Trainer : GymUser
    {
        //hiredate == Createdat ==>base entity
        public Specialities Specialities { get; set; }

        public ICollection<Session> TrainerSession { get; set; } = null!;
    }
}
