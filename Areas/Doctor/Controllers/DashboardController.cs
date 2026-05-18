using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class DashboardController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(IAppointmentService appointmentService, IDoctorService doctorService, UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var doctor = doctors.FirstOrDefault(d => d.ApplicationUserId == user.Id);
            
            if (doctor == null) return NotFound("Doctor profile not found.");

            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.Id);
            
            ViewBag.TodayAppointments = appointments.Where(a => a.AppointmentDate.Date == DateTime.Today);
            ViewBag.UpcomingAppointments = appointments.Where(a => a.AppointmentDate > DateTime.Now).OrderBy(a => a.AppointmentDate).Take(10);
            ViewBag.TotalAppointments = appointments.Count();
            ViewBag.CompletedAppointments = appointments.Count(a => a.Status == Models.Enums.AppointmentStatus.Completed);

            return View(doctor);
        }

        public async Task<IActionResult> Patients()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var doctor = doctors.FirstOrDefault(d => d.ApplicationUserId == user.Id);
            
            if (doctor == null) return NotFound("Doctor profile not found.");

            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.Id);
            return View(appointments);
        }

        public async Task<IActionResult> Schedules()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var doctors = await _doctorService.GetAllDoctorsAsync();
            var doctor = doctors.FirstOrDefault(d => d.ApplicationUserId == user.Id);
            
            if (doctor == null) return NotFound("Doctor profile not found.");

            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.Id);
            return View(appointments.OrderByDescending(a => a.AppointmentDate));
        }

        public IActionResult AIInsights()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult Emergency()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
