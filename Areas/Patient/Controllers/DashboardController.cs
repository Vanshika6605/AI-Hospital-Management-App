using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public class DashboardController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IBillingService _billingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IAppointmentService appointmentService, 
            IPatientService patientService, 
            IPrescriptionService prescriptionService,
            IBillingService billingService,
            UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _prescriptionService = prescriptionService;
            _billingService = billingService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var patients = await _patientService.GetAllPatientsAsync();
            var patient = patients.FirstOrDefault(p => p.ApplicationUserId == user.Id);
            
            if (patient == null) return NotFound("Patient profile not found.");

            var appointments = await _appointmentService.GetPatientAppointmentsAsync(patient.Id);
            var prescriptions = await _prescriptionService.GetPatientPrescriptionsAsync(patient.Id);
            var bills = await _billingService.GetAllBillsAsync();
            var patientBills = bills.Where(b => b.PatientName == patient.FullName); // In real app, filter by PatientId in service

            ViewBag.UpcomingAppointments = appointments.Where(a => a.AppointmentDate > DateTime.Now).OrderBy(a => a.AppointmentDate).Take(5);
            ViewBag.RecentPrescriptions = prescriptions.OrderByDescending(p => p.CreatedAt).Take(5);
            ViewBag.PendingBills = patientBills.Where(b => b.PaymentStatus != "Paid");

            return View(patient);
        }
    }
}
