using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorViewModel>> GetAllDoctorsAsync(string? search = null, int? departmentId = null);
        Task<DoctorViewModel?> GetDoctorByIdAsync(int id);
        Task CreateDoctorAsync(DoctorViewModel model);
        Task UpdateDoctorAsync(DoctorViewModel model);
        Task DeleteDoctorAsync(int id);
    }
}
