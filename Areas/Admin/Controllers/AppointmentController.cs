using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Receptionist")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        public async Task<IActionResult> Book()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _patientService.GetAllPatientsAsync();
            
            ViewBag.DoctorId = new SelectList(doctors, "Id", "FullName");
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName");
            
            return View(new AppointmentViewModel { AppointmentDate = DateTime.Now.AddDays(1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appointmentService.BookAppointmentAsync(model);
                    TempData["Success"] = "Appointment booked successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _patientService.GetAllPatientsAsync();
            ViewBag.DoctorId = new SelectList(doctors, "Id", "FullName", model.DoctorId);
            ViewBag.PatientId = new SelectList(patients, "Id", "FullName", model.PatientId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(id, status);
            TempData["Success"] = $"Appointment status updated to {status}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _appointmentService.CancelAppointmentAsync(id);
            TempData["Success"] = "Appointment cancelled successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int doctorId, DateTime date)
        {
            bool isAvailable = await _appointmentService.CheckAvailabilityAsync(doctorId, date);
            return Json(new { available = isAvailable });
        }
    }
}
