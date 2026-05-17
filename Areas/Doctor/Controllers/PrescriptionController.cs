using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor,Admin")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IAppointmentService _appointmentService;

        public PrescriptionController(IPrescriptionService prescriptionService, IAppointmentService appointmentService)
        {
            _prescriptionService = prescriptionService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Create(int appointmentId)
        {
            var model = await _prescriptionService.GetPrescriptionByAppointmentIdAsync(appointmentId);
            if (model == null) return NotFound();
            
            if (model.Id > 0)
            {
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _prescriptionService.CreatePrescriptionAsync(model);
                // Also mark appointment as completed
                await _appointmentService.UpdateAppointmentStatusAsync(model.AppointmentId, "Completed");
                
                TempData["Success"] = "Prescription added successfully";
                return RedirectToAction("Index", "Appointment", new { area = "Admin" }); // Or Doctor Dashboard
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null) return NotFound();
            return View(prescription);
        }
    }
}
