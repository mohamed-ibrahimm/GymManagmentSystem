using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentDAL.REpostitory.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDBContext _dBContext;

        public SessionRepository(GymDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext;
        }

        public IEnumerable<Session> GetAllSesiionWithTrainerAndCategory()
        {
            return _dBContext.Sessions
                             .Include(x => x.SessionTrainer)
                             .Include(x => x.SessionCategory)
                             .ToList();
        }

        public int GetCountofBookedSlot(int sessionid)
        {
            return _dBContext.MembersSessions.Count(x => x.SessionId == sessionid);
        }

        public Session? GetSessionWithTrainerandCategory(int sessionid)
        {
            return _dBContext.Sessions
                             .Include(x => x.SessionTrainer)
                             .Include(x => x.SessionCategory)
                             .FirstOrDefault(x => x.Id == sessionid);
        }
    }
}