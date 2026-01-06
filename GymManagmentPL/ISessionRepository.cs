using GymManagmentDAL.Entities;

namespace GymManagmentDAL.REpostitory.Interfaces
{
    public interface IsessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSesiionWithTrainerAndCategory();

        int GetCountofBookedSlot(int sessionid);

        Session? GetSessionWithTrainerandCategory(int sessionid);
    }
}
