using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientViewModel>> GetAllPatientsAsync(string? search = null);
        Task<PatientViewModel?> GetPatientByIdAsync(int id);
        Task CreatePatientAsync(PatientViewModel model);
        Task UpdatePatientAsync(PatientViewModel model);
        Task DeletePatientAsync(int id);
    }
}
