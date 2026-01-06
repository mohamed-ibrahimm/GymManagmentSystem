using GymManagmentBLL.Service.Interfaces;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;

namespace GymManagmentBLL.Service.Classes
{
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberSessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string BookSession(MemberSession memberSession)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(memberSession.SessionId);
            if (session == null) return "Session not found.";

            if (session.StartDate <= DateTime.Now)
                return "Cannot book a past or started session.";

            var hasActiveMembership = _unitOfWork.GetRepository<MemberShip>()
                .GetAll()
                .Any(m => m.MemberID == memberSession.MemberId && m.EndDate >= DateTime.Now);

            if (!hasActiveMembership)
                return "Member does not have an active membership.";

            var isAlreadyBooked = _unitOfWork.GetRepository<MemberSession>()
                .GetAll()
                .Any(ms => ms.SessionId == memberSession.SessionId && ms.MemberId == memberSession.MemberId);

            if (isAlreadyBooked)
                return "Member is already booked in this session.";

            var currentBookingsCount = _unitOfWork.GetRepository<MemberSession>()
                .GetAll()
                .Count(ms => ms.SessionId == memberSession.SessionId);

            int sessionCapacity = 20;

            if (currentBookingsCount >= sessionCapacity)
                return "Session is full.";

            memberSession.ISAttended = false;

            _unitOfWork.GetRepository<MemberSession>().Add(memberSession);
            _unitOfWork.SaveChange();

            return "Success";
        }

        public string Cancel(int id)
        {
            var booking = _unitOfWork.GetRepository<MemberSession>().GetById(id);
            if (booking == null) return "Booking not found";

            var session = _unitOfWork.GetRepository<Session>().GetById(booking.SessionId);

            if (session != null && session.StartDate <= DateTime.Now)
            {
                return "Cannot cancel a session that has already started.";
            }

            _unitOfWork.GetRepository<MemberSession>().Delete(booking);
            _unitOfWork.SaveChange();
            return "Success";
        }

        public void Create(MemberSession session)
        {
            _unitOfWork.GetRepository<MemberSession>().Add(session);
            _unitOfWork.SaveChange();
        }

        public void Delete(MemberSession session)
        {
            _unitOfWork.GetRepository<MemberSession>().Delete(session);
            _unitOfWork.SaveChange();
        }

        public IEnumerable<MemberSession> GetAll()
        {
            return _unitOfWork.GetRepository<MemberSession>().GetAll();
        }

        public MemberSession? GetById(int id)
        {
            return _unitOfWork.GetRepository<MemberSession>().GetById(id);
        }

        public IEnumerable<Member> GetMembersForOngoingSession(int sessionId)
        {
            return _unitOfWork.GetRepository<MemberSession>()
                .GetAll()
                .Where(ms => ms.SessionId == sessionId && ms.Session.StartDate <= DateTime.Now && ms.Session.EndDate >= DateTime.Now)
                .Select(ms => ms.Member);
        }

        public IEnumerable<Member> GetMembersForUpcomingSession(int sessionId)
        {
            return _unitOfWork.GetRepository<MemberSession>()
                .GetAll()
                .Where(ms => ms.SessionId == sessionId && ms.Session.StartDate > DateTime.Now)
                .Select(ms => ms.Member);
        }

        public IEnumerable<MemberSession> GetOngoingSessions()
        {
            return _unitOfWork.GetRepository<MemberSession>().GetAll()
                .Where(s => s.Session != null && s.Session.StartDate <= DateTime.Now && s.Session.EndDate >= DateTime.Now);
        }

        public IEnumerable<MemberSession> GetUpcomingSessions()
        {
            return _unitOfWork.GetRepository<MemberSession>().GetAll()
                .Where(s => s.Session != null && s.Session.StartDate > DateTime.Now);
        }

        public string MarkAttendance(int bookingId)
        {
            var booking = _unitOfWork.GetRepository<MemberSession>().GetById(bookingId);
            if (booking == null) return "Booking not found.";

            var session = _unitOfWork.GetRepository<Session>().GetById(booking.SessionId);

            if (session == null) return "Session not found.";

            var now = DateTime.Now;
            if (session.StartDate > now || session.EndDate < now)
            {
                return "Attendance can only be marked for ongoing sessions.";
            }

            booking.ISAttended = true;
            _unitOfWork.GetRepository<MemberSession>().Update(booking);
            _unitOfWork.SaveChange();

            return "Success";
        }
    }
}