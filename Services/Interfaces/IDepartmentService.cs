using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentViewModel>> GetAllDepartmentsAsync();
        Task<DepartmentViewModel?> GetDepartmentByIdAsync(int id);
        Task CreateDepartmentAsync(DepartmentViewModel model);
        Task UpdateDepartmentAsync(DepartmentViewModel model);
        Task DeleteDepartmentAsync(int id);
    }
}
