using GymManagmentBLL.Service.Interfaces;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;

namespace GymManagmentBLL.Service.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Create(MemberShip membership)
        {

            var plan = _unitOfWork.GetRepository<Plane>().GetById(membership.PlaneId);
            if (plan == null) return "Plan not found.";
            if (!plan.IsActive) return "Cannot assign an inactive plan.";

            var member = _unitOfWork.GetRepository<Member>().GetById(membership.MemberID);
            if (member == null) return "Member not found.";

            var hasActiveMembership = _unitOfWork.GetRepository<MemberShip>()
                .GetAll()
                .Any(m => m.MemberID == membership.MemberID && m.EndDate > DateTime.Now);

            if (hasActiveMembership)
                return "Member already has an active membership.";


            membership.EndDate = DateTime.Now.AddDays(plan.DurationDays);


            _unitOfWork.GetRepository<MemberShip>().Add(membership);
            _unitOfWork.SaveChange();

            return "Success";
        }

        public string Cancel(int id)
        {
            var membership = _unitOfWork.GetRepository<MemberShip>().GetById(id);
            if (membership == null) return "Membership not found.";

            if (membership.EndDate <= DateTime.Now)
            {
                return "Cannot delete/cancel an expired membership.";
            }

            _unitOfWork.GetRepository<MemberShip>().Delete(membership);
            _unitOfWork.SaveChange();

            return "Success";
        }

        public IEnumerable<MemberShip> GetAll()
        {
            return _unitOfWork.GetRepository<MemberShip>().GetAll();
        }

        public MemberShip? GetById(int id)
        {
            return _unitOfWork.GetRepository<MemberShip>().GetById(id);
        }

        public void Update(MemberShip entity)
        {
            _unitOfWork.GetRepository<MemberShip>().Update(entity);
            _unitOfWork.SaveChange();
        }
    }
}