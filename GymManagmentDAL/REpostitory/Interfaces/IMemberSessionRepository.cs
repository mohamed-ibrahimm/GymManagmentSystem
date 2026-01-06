using GymManagmentDAL.Entities;
using System.Linq.Expressions;

namespace GymManagmentDAL.REpostitory.Interfaces
{
    public interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetAll(Expression<Func<MemberSession, bool>> filter);



        MemberSession? GetById(int id);
        void Create(MemberSession session);
        void Delete(MemberSession session);

        IEnumerable<Member> GetMembersForUpcomingSession(int sessionId);
        IEnumerable<Member> GetMembersForOngoingSession(int sessionId);
        IEnumerable<MemberSession> GetAll();
    }
}
