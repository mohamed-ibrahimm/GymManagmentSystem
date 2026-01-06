using GymManagmentDAL.Entities;

namespace GymManagmentBLL.Service.Interfaces
{
    public interface IMemberShipService
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int id);

        string Create(MemberShip membership);
        string Cancel(int id);

        void Update(MemberShip entity);
    }
}