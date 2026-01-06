using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.SessionViewModel;
using GymManagmentDAL.Entities;
using GymManagmentDAL.REpostitory.Interfaces;

namespace GymManagmentBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {

            if (!ISTrainerExist(createSession.TrainerId))
                throw new Exception("Trainer does not exist");

            if (!ISCategoryExist(createSession.CategoryId))
                throw new Exception("Category does not exist");

            if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate))
                throw new Exception("Start date must be before end date");

            if (createSession.Capacity > 100 || createSession.Capacity < 1)
                throw new Exception("Capacity must be between 1 and 100");


            var seesionEntity = _mapper.Map<Session>(createSession);


            seesionEntity.Status = "Upcoming";

            _unitOfWork.GetRepository<Session>().Add(seesionEntity);


            return _unitOfWork.SaveChange() > 0;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.sessionRepository.GetAllSesiionWithTrainerAndCategory();
            if (!sessions.Any()) return new List<SessionViewModel>();

            var MappedSession = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach (var Session in MappedSession)
            {

                Session.AvailableSlot = Session.Capacity - _unitOfWork.sessionRepository.GetCountofBookedSlot(Session.Id);
            }
            return MappedSession;
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainer);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var category = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(category);
        }

        public SessionViewModel? GetSessionById(int sessionid)
        {
            var Session = _unitOfWork.sessionRepository.GetSessionWithTrainerandCategory(sessionid);
            if (Session is null) return null;
            var mappedSession = _mapper.Map<Session, SessionViewModel>(Session);

            mappedSession.AvailableSlot = mappedSession.Capacity - _unitOfWork.sessionRepository.GetCountofBookedSlot(sessionid);
            return mappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionid)
        {
            var Session = _unitOfWork.sessionRepository.GetById(sessionid);
            if (!IsSessionAvailbletoupdate(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionid)
        {
            var session = _unitOfWork.sessionRepository.GetById(sessionid);

            if (session == null) return false;


            if (!IsSessionAvailbletoupdate(session)) return false;

            if (!ISTrainerExist(updateSession.TrainerId)) return false;

            if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;

            _mapper.Map(updateSession, session);

            _unitOfWork.sessionRepository.Update(session);

            return _unitOfWork.SaveChange() > 0;
        }

        public bool RemoveSession(int sessionid)
        {
            var session = _unitOfWork.sessionRepository.GetById(sessionid);
            if (!ISSessionavialbleforRemoving(session!)) return false;
            _unitOfWork.sessionRepository.Delete(session!);
            return _unitOfWork.SaveChange() > 0;
        }

        #region Helper
        private bool ISTrainerExist(int trainerid)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerid) is not null;
        }
        private bool ISCategoryExist(int categoryid)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryid) is not null;
        }
        private bool IsDateTimeValid(DateTime startdate, DateTime enddate)
        {
            return startdate < enddate;
        }

        private bool IsSessionAvailbletoupdate(Session session)
        {
            if (session is null) return false;
            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now) return false;
            var hasactivebooking = _unitOfWork.sessionRepository.GetCountofBookedSlot(session.Id) > 0;
            if (hasactivebooking) return false;
            return true;
        }

        private bool ISSessionavialbleforRemoving(Session session)
        {
            if (session is null) return false;


            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;


            if (session.EndDate < DateTime.Now) return false;


            var hasactivebooking = _unitOfWork.sessionRepository.GetCountofBookedSlot(session.Id) > 0;
            if (hasactivebooking) return false;

            return true;
        }
        #endregion
    }
}