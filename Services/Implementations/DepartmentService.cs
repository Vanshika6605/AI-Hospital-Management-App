using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _departmentRepo;

        public DepartmentService(IGenericRepository<Department> departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public async Task<IEnumerable<DepartmentViewModel>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepo.GetAllAsync();
            return departments.Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt
            });
        }

        public async Task<DepartmentViewModel?> GetDepartmentByIdAsync(int id)
        {
            var d = await _departmentRepo.GetByIdAsync(id);
            if (d == null) return null;

            return new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt
            };
        }

        public async Task CreateDepartmentAsync(DepartmentViewModel model)
        {
            var department = new Department
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _departmentRepo.AddAsync(department);
            await _departmentRepo.SaveAsync();
        }

        public async Task UpdateDepartmentAsync(DepartmentViewModel model)
        {
            var department = await _departmentRepo.GetByIdAsync(model.Id);
            if (department != null)
            {
                department.Name = model.Name;
                department.Description = model.Description;
                department.UpdatedAt = DateTime.UtcNow;

                _departmentRepo.Update(department);
                await _departmentRepo.SaveAsync();
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department != null)
            {
                _departmentRepo.Remove(department);
                await _departmentRepo.SaveAsync();
            }
        }
    }
}
