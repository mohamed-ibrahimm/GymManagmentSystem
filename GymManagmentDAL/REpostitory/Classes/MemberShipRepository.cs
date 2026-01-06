using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentDAL.REpostitory.Classes
{
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDBContext _context;

        public MemberShipRepository(GymDBContext context)
        {
            _context = context;
        }

        public void Add(MemberShip entity)
        {
            _context.MembersShips.Add(entity);
            Save();
        }

        public void Cancel(int id)
        {
            var MemberShip = GetById(id);
            if (MemberShip == null) return;

            MemberShip.EndDate = DateTime.Now;
            Save();
        }

        public IEnumerable<MemberShip> GetAll()
        {
            return _context.MembersShips
                .Include(ms => ms.Member)
                .Include(ms => ms.Plane)
                .ToList();

        }

        public MemberShip? GetById(int id)
        {
            return _context.MembersShips
                .Include(x => x.Member)
                .Include(x => x.Plane)
                .FirstOrDefault(x => x.MemberID == id);
        }



        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(MemberShip entity)
        {
            _context.MembersShips.Update(entity);
            Save();
        }
    }
}
