using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Receptionist")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var patients = await _patientService.GetAllPatientsAsync(search);
            ViewBag.Search = search;
            return View(patients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _patientService.CreatePatientAsync(model);
                    TempData["Success"] = "Patient registered successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _patientService.UpdatePatientAsync(model);
                TempData["Success"] = "Patient profile updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientService.DeletePatientAsync(id);
            TempData["Success"] = "Patient record deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
