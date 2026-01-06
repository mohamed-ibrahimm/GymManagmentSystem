using GymManagmentDAL.Entities;

namespace GymManagmentDAL.REpostitory.Interfaces
{
    public interface IMemberShipRepository
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int id);

        void Add(MemberShip entity);
        void Update(MemberShip entity);
        void Cancel(int id);

        void Save();
    }
}
