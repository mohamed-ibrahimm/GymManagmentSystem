using GymManagmentDAL.Entities;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberSessionService
    {
        IEnumerable<MemberSession> GetAll();
        MemberSession? GetById(int id);
        string BookSession(MemberSession session);
        void Create(MemberSession session);
        string Cancel(int id);
        void Delete(MemberSession session);
        IEnumerable<Member> GetMembersForUpcomingSession(int sessionId);
        IEnumerable<Member> GetMembersForOngoingSession(int sessionId);
        IEnumerable<MemberSession> GetOngoingSessions();
        IEnumerable<MemberSession> GetUpcomingSessions();
        string MarkAttendance(int bookingId);
    }
}