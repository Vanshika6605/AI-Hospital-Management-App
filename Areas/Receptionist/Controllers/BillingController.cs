using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [Authorize(Roles = "Admin,Receptionist")]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;
        private readonly IAppointmentService _appointmentService;

        public BillingController(IBillingService billingService, IAppointmentService appointmentService)
        {
            _billingService = billingService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _billingService.GetAllBillsAsync();
            return View(bills);
        }

        public async Task<IActionResult> Create(int appointmentId)
        {
            var model = await _billingService.GetBillByAppointmentIdAsync(appointmentId);
            if (model == null) return NotFound();
            
            if (model.Id > 0)
            {
                return RedirectToAction(nameof(Invoice), new { id = model.Id });
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _billingService.GenerateBillAsync(model);
                TempData["Success"] = "Bill generated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Invoice(int id)
        {
            var bill = await _billingService.GetBillByIdAsync(id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPaid(int id)
        {
            await _billingService.UpdatePaymentStatusAsync(id, "Paid");
            TempData["Success"] = "Payment recorded successfully";
            return RedirectToAction(nameof(Invoice), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPending(int id)
        {
            await _billingService.UpdatePaymentStatusAsync(id, "Pending");
            TempData["Success"] = "Bill finalized and marked as Pending";
            return RedirectToAction(nameof(Index));
        }
    }
}
