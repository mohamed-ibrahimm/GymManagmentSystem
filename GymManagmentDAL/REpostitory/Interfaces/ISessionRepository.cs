using GymManagmentDAL.Entities;

namespace GymManagmentDAL.REpostitory.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSesiionWithTrainerAndCategory();
        int GetCountofBookedSlot(int sessionid);
        Session? GetSessionWithTrainerandCategory(int sessionid);
    }
}