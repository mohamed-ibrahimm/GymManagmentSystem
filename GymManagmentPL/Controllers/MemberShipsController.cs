using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.ViewModels.MembershipIndexViewModel;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class MemberShipsController : Controller
    {
        private readonly IMemberShipService _service;
        private readonly IMemberService _memberService;
        private readonly IPlanServicecs _planService;

        public MemberShipsController(
            IMemberShipService service,
            IMemberService memberService,
            IPlanServicecs planService)
        {
            _service = service;
            _memberService = memberService;
            _planService = planService;
        }

        public IActionResult Index()
        {
            var data = _service.GetAll()
                .Select(x => new MembershipIndexViewModel
                {
                    Id = x.Id,

                    MemberName = x.Member?.Name ?? "unknown",
                    PlanName = x.Plane?.Name ?? "unknown",

                    StartDate = x.CreatedAt,
                    EndDate = x.EndDate,
                    Status = x.EndDate > DateTime.Now ? "Active" : "Expired"
                });

            return View(data);
        }

        public IActionResult Create()
        {
            var vm = new CreateMembershipViewModel
            {
                Members = _memberService.GetAllMember()
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }),

                Plans = _planService.GetAllPlane()
                    .Where(p => p.IsActive)
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Name} ({p.DurationDayes} Days)" })
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CreateMembershipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = _memberService.GetAllMember()
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });

                model.Plans = _planService.GetAllPlane()
                    .Where(p => p.IsActive)
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });

                return View(model);
            }

            var entity = new MemberShip
            {
                MemberID = model.MemberId,
                PlaneId = model.PlaneId,
            };


            string result = _service.Create(entity);

            if (result == "Success")
            {
                TempData["Success"] = "Membership created successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result);

            model.Members = _memberService.GetAllMember()
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });
            model.Plans = _planService.GetAllPlane()
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();


            var vm = new MembershipIndexViewModel
            {
                Id = entity.Id,
                MemberName = entity.Member?.Name ?? "Unknown",
                PlanName = entity.Plane?.Name ?? "Unknown",
                StartDate = entity.CreatedAt,
                EndDate = entity.EndDate,
                Status = entity.EndDate > DateTime.Now ? "Active" : "Expired"
            };

            return View(vm);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            string result = _service.Cancel(id);

            if (result == "Success")
            {
                TempData["Success"] = "Membership deleted successfully";
            }
            else
            {
                TempData["Error"] = result;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var vm = new CreateMembershipViewModel
            {
                MemberId = entity.MemberID,
                PlaneId = entity.PlaneId,
                EndDate = entity.EndDate,
                Members = _memberService.GetAllMember()
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }),
                Plans = _planService.GetAllPlane()
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, CreateMembershipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = _memberService.GetAllMember()
                     .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });
                model.Plans = _planService.GetAllPlane()
                     .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });
                return View(model);
            }

            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            entity.MemberID = model.MemberId;
            entity.PlaneId = model.PlaneId;


            _service.Update(entity);

            return RedirectToAction(nameof(Index));
        }
    }
}