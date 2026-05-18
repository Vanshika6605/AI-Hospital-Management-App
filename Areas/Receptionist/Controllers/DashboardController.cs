using AIHospitalManagementSys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AIHospitalManagementSys.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [Authorize(Roles = "Receptionist")]
    public class DashboardController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IBillingService _billingService;

        public DashboardController(IAppointmentService appointmentService, IBillingService billingService)
        {
            _appointmentService = appointmentService;
            _billingService = billingService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var bills = await _billingService.GetAllBillsAsync();

            var todayAppointments = appointments
                .Where(a => a.AppointmentDate.Date == DateTime.Today)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            var draftBills = bills
                .Where(b => b.PaymentStatus == "Draft")
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            ViewBag.TodayAppointmentsCount = todayAppointments.Count;
            ViewBag.PendingConfirmationsCount = appointments.Count(a => a.Status == Models.Enums.AppointmentStatus.Pending);
            ViewBag.DraftBillsCount = draftBills.Count;
            ViewBag.CompletedConsultationsCount = appointments.Count(a => a.Status == Models.Enums.AppointmentStatus.Completed);
            
            ViewBag.DraftBills = draftBills.Take(5).ToList();

            return View(todayAppointments);
        }
    }
}
