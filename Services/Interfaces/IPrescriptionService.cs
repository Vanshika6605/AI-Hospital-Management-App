using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionViewModel?> GetPrescriptionByAppointmentIdAsync(int appointmentId);
        Task<PrescriptionViewModel?> GetPrescriptionByIdAsync(int id);
        Task CreatePrescriptionAsync(PrescriptionViewModel model);
        Task<IEnumerable<PrescriptionViewModel>> GetPatientPrescriptionsAsync(int patientId);
    }
}
