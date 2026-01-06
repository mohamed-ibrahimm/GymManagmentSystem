using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagmentDAL.REpostitory.Classes
{
    public class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDBContext _context;

        public MemberSessionRepository(GymDBContext context)
        {
            _context = context;
        }

        public void Create(MemberSession session)
        {
            _context.Add(session);
            _context.SaveChanges();
        }

        public void Delete(MemberSession session)
        {
            _context.Remove(session);
            _context.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll(Expression<Func<MemberSession, bool>> value)
        {
            return _context.MembersSessions
              .Include(x => x.Member)
              .Include(x => x.Session)
              .Where(value)
              .ToList();
        }

        public IEnumerable<MemberSession> GetAll() => _context.MembersSessions
              .Include(x => x.Member)
              .Include(x => x.Session);

        public MemberSession? GetById(int id) => _context.MembersSessions
            .Include(x => x.Member)
            .Include(x => x.Session)
            .FirstOrDefault(x => x.Id == id);

        public IEnumerable<Member> GetMembersForOngoingSession(int sessionId) => _context.MembersSessions
            .Where(x => x.SessionId == sessionId && x.Session.Status == "Ongoing")
            .Select(x => x.Member)
            .ToList();

        public IEnumerable<Member> GetMembersForUpcomingSession(int sessionId) => _context.MembersSessions
            .Where(x => x.SessionId == sessionId && (x.Session.Status == "Upcoming" || x.Session.StartDate > DateTime.Now))
            .Select(x => x.Member)
            .ToList();
    }
}