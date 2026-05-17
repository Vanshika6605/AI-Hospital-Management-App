using AIHospitalManagementSys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDepartmentService _departmentService;

        public DashboardController(
            IDoctorService doctorService, 
            IPatientService patientService, 
            IAppointmentService appointmentService,
            IDepartmentService departmentService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            var patients = await _patientService.GetAllPatientsAsync();
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var departments = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.TotalDoctors = doctors.Count();
            ViewBag.TotalPatients = patients.Count();
            ViewBag.TotalAppointments = appointments.Count();
            ViewBag.TotalDepartments = departments.Count();
            
            ViewBag.RecentAppointments = appointments.OrderByDescending(a => a.CreatedAt).Take(5);
            
            // Stats for Charts (Mock data for now until revenue/daily stats implemented)
            ViewBag.MonthlyAppointmentLabels = "['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun']";
            ViewBag.MonthlyAppointmentData = "[12, 19, 3, 5, 2, 3]";

            return View();
        }
    }
}
