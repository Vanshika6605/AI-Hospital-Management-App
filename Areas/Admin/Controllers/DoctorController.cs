using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IDepartmentService _departmentService;

        public DoctorController(IDoctorService doctorService, IDepartmentService departmentService)
        {
            _doctorService = doctorService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index(string? search, int? departmentId)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync(search, departmentId);
            var departments = await _departmentService.GetAllDepartmentsAsync();
            
            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);
            ViewBag.Search = search;
            
            return View(doctors);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _doctorService.CreateDoctorAsync(model);
                    TempData["Success"] = "Doctor created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", model.DepartmentId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", doctor.DepartmentId);
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _doctorService.UpdateDoctorAsync(model);
                TempData["Success"] = "Doctor updated successfully";
                return RedirectToAction(nameof(Index));
            }

            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", model.DepartmentId);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _doctorService.DeleteDoctorAsync(id);
            TempData["Success"] = "Doctor deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
