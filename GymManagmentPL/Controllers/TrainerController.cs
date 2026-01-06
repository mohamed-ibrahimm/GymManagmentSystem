using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        public ActionResult TrainerDetails(int ID)
        {
            if (ID <= 0)
            {
                TempData["ErrorMessage"] = "ID Can't Be 0 Or Less";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(ID);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Data & Missing Field");
                return View(nameof(Create), createTrainer);
            }
            bool result = _trainerService.CreateTrainer(createTrainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Added Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Can't Be Added.";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(int ID)
        {
            if (ID <= 0)
            {
                TempData["ErrorMessage"] = "ID Can't Be 0 Or Less";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(ID);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel trainerUpdate)
        {
            if (!ModelState.IsValid)
                return View(trainerUpdate);
            var result = _trainerService.UpdateTrainerDetails(trainerUpdate, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Can't Be Updated.";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int ID)
        {
            if (ID <= 0)
            {
                ModelState.AddModelError("DataMissed", "Check Data & Missing Field");
                return RedirectToAction(nameof(Index));
            }
            var member = _trainerService.GetTrainerDetails(ID);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ID = ID;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm([FromForm] int ID)
        {
            var result = _trainerService.RemoveTrainer(ID);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Removed Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Can't Be Removed.";
            }
            ViewBag.ID = ID;
            return RedirectToAction(nameof(Index));
        }
    }
}