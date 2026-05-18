using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Models.Enums;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly INotificationService _notificationService;
        private readonly IGenericRepository<Patient> _patientRepo;
        private readonly IGenericRepository<Doctor> _doctorRepo;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            INotificationService notificationService,
            IGenericRepository<Patient> patientRepo,
            IGenericRepository<Doctor> doctorRepo)
        {
            _appointmentRepo = appointmentRepo;
            _notificationService = notificationService;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentRepo.GetAllAsync(includeProperties: "Doctor.ApplicationUser,Doctor.Department,Patient.ApplicationUser");
            return appointments.Select(a => MapToViewModel(a));
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var appointments = await _appointmentRepo.GetAppointmentsByDoctorIdAsync(doctorId);
            return appointments.Select(a => MapToViewModel(a));
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetPatientAppointmentsAsync(int patientId)
        {
            var appointments = await _appointmentRepo.GetAppointmentsByPatientIdAsync(patientId);
            return appointments.Select(a => MapToViewModel(a));
        }

        public async Task<AppointmentViewModel?> GetAppointmentByIdAsync(int id)
        {
            var a = await _appointmentRepo.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "Doctor.ApplicationUser,Doctor.Department,Patient.ApplicationUser");
            if (a == null) return null;
            return MapToViewModel(a);
        }

        public async Task BookAppointmentAsync(AppointmentViewModel model)
        {
            bool isAvailable = await _appointmentRepo.IsDoctorAvailableAsync(model.DoctorId, model.AppointmentDate);
            if (!isAvailable)
            {
                throw new Exception("Doctor is not available at the requested time slot.");
            }

            var appointment = new Appointment
            {
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                AppointmentDate = model.AppointmentDate,
                Reason = model.Reason,
                Status = AppointmentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveAsync();

            var patient = await _patientRepo.GetFirstOrDefaultAsync(p => p.Id == model.PatientId, "ApplicationUser");
            var doctor = await _doctorRepo.GetFirstOrDefaultAsync(d => d.Id == model.DoctorId, "ApplicationUser");

            if (patient?.ApplicationUserId != null)
            {
                await _notificationService.SendNotificationAsync(patient.ApplicationUserId, $"Your appointment is booked for {model.AppointmentDate:MMM dd, yyyy hh:mm tt}.");
            }
            if (doctor?.ApplicationUserId != null)
            {
                await _notificationService.SendNotificationAsync(doctor.ApplicationUserId, $"New appointment booked by {patient?.ApplicationUser?.FullName} for {model.AppointmentDate:MMM dd, yyyy hh:mm tt}.");
            }
        }

        public async Task UpdateAppointmentStatusAsync(int id, string status)
        {
            var appointment = await _appointmentRepo.GetFirstOrDefaultAsync(a => a.Id == id, "Patient,Doctor");
            if (appointment != null)
            {
                if (Enum.TryParse<AppointmentStatus>(status, out var newStatus))
                {
                    appointment.Status = newStatus;
                    appointment.UpdatedAt = DateTime.UtcNow;
                    _appointmentRepo.Update(appointment);
                    await _appointmentRepo.SaveAsync();

                    if (appointment.Patient != null)
                    {
                        var patient = await _patientRepo.GetFirstOrDefaultAsync(p => p.Id == appointment.PatientId);
                        if (patient?.ApplicationUserId != null)
                        {
                            await _notificationService.SendNotificationAsync(patient.ApplicationUserId, $"Your appointment status has been updated to: {status}.");
                        }
                    }
                }
            }
        }

        public async Task CancelAppointmentAsync(int id)
        {
            await UpdateAppointmentStatusAsync(id, "Cancelled");
        }

        public async Task<bool> CheckAvailabilityAsync(int doctorId, DateTime date)
        {
            return await _appointmentRepo.IsDoctorAvailableAsync(doctorId, date);
        }

        private AppointmentViewModel MapToViewModel(Appointment a)
        {
            return new AppointmentViewModel
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.ApplicationUser?.FullName,
                DepartmentName = a.Doctor?.Department?.Name,
                PatientId = a.PatientId,
                PatientName = a.Patient?.ApplicationUser?.FullName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            };
        }
    }
}
