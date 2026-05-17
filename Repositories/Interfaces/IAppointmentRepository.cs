using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;

namespace AIHospitalManagementSys.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
    }
}
