using AutoMapper;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MemberSessionIndexViewModel;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class MemberSessionController : Controller
    {
        private readonly IMemberSessionService _service;
        private readonly IMemberService _memberService;
        private readonly ISessionService _sessionService;
        private readonly IMapper _mapper;

        public MemberSessionController(
            IMemberSessionService service,
            IMemberService memberService,
            ISessionService sessionService,
            IMapper mapper)
        {
            _service = service;
            _memberService = memberService;
            _sessionService = sessionService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var upcomingData = _service.GetUpcomingSessions();
            var ongoingData = _service.GetOngoingSessions();

            var upcomingSessions = upcomingData.Select(ms => ms.Session).DistinctBy(s => s.Id);
            var ongoingSessions = ongoingData.Select(ms => ms.Session).DistinctBy(s => s.Id);

            var model = new MemberSessionIndexViewModel
            {
                Upcoming = _mapper.Map<IEnumerable<SessionCardViewModel>>(upcomingSessions),
                Ongoing = _mapper.Map<IEnumerable<SessionCardViewModel>>(ongoingSessions)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(int? sessionId)
        {

            var viewModel = new CreateMemberSessionViewModel();


            if (sessionId.HasValue)
            {
                viewModel.SessionId = sessionId.Value;
            }


            viewModel.Members = _memberService.GetAllMember()
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();


            ViewBag.Sessions = new SelectList(_sessionService.GetAllSessions(), "Id", "SessionCategory.CategoryName", sessionId);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateMemberSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = _memberService.GetAllMember()
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name
                    }).ToList();

                ViewBag.Sessions = new SelectList(_sessionService.GetAllSessions(), "Id", "SessionCategory.CategoryName", model.SessionId);

                return View(model);
            }

            var entity = _mapper.Map<MemberSession>(model);
            string result = _service.BookSession(entity);

            if (result == "Success")
            {
                TempData["Success"] = "Booking created successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result);

            model.Members = _memberService.GetAllMember()
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();

            ViewBag.Sessions = new SelectList(_sessionService.GetAllSessions(), "Id", "SessionCategory.CategoryName", model.SessionId);

            return View(model);
        }

        public IActionResult Cancel(int id)
        {
            string result = _service.Cancel(id);
            if (result == "Success") TempData["Success"] = "Booking cancelled successfully";
            else TempData["Error"] = result;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult MarkAttendance(int id)
        {
            string result = _service.MarkAttendance(id);
            if (result == "Success") TempData["Success"] = "Attendance marked successfully.";
            else TempData["Error"] = result;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetMembersForUpcomingSession(int sessionId)
        {
            var members = _service.GetMembersForUpcomingSession(sessionId);
            ViewBag.SessionId = sessionId;
            return View(members);
        }

        public IActionResult GetMembersForOngoingSessions(int sessionId)
        {
            var members = _service.GetMembersForOngoingSession(sessionId);
            return View(members);
        }
    }
}