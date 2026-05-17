using AIHospitalManagementSys.Data;
using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AIHospitalManagementSys.Repositories.Implementations
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate)
        {
            // Check if there's any appointment within 30 mins of the requested time
            var startTime = appointmentDate.AddMinutes(-29);
            var endTime = appointmentDate.AddMinutes(29);

            return !await _context.Appointments.AnyAsync(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate > startTime && 
                a.AppointmentDate < endTime && 
                a.Status != Models.Enums.AppointmentStatus.Cancelled);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.ApplicationUser)
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.ApplicationUser)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
