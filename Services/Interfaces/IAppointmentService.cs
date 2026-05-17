using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentViewModel>> GetAllAppointmentsAsync();
        Task<IEnumerable<AppointmentViewModel>> GetDoctorAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentViewModel>> GetPatientAppointmentsAsync(int patientId);
        Task<AppointmentViewModel?> GetAppointmentByIdAsync(int id);
        Task BookAppointmentAsync(AppointmentViewModel model);
        Task UpdateAppointmentStatusAsync(int id, string status);
        Task CancelAppointmentAsync(int id);
        Task<bool> CheckAvailabilityAsync(int doctorId, DateTime date);
    }
}
