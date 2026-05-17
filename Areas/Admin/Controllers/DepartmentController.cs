using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.CreateDepartmentAsync(model);
                TempData["Success"] = "Department created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateDepartmentAsync(model);
                TempData["Success"] = "Department updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            TempData["Success"] = "Department deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
