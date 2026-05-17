using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IGenericRepository<Prescription> _prescriptionRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;

        public PrescriptionService(IGenericRepository<Prescription> prescriptionRepo, IGenericRepository<Appointment> appointmentRepo)
        {
            _prescriptionRepo = prescriptionRepo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<PrescriptionViewModel?> GetPrescriptionByAppointmentIdAsync(int appointmentId)
        {
            var p = await _prescriptionRepo.GetFirstOrDefaultAsync(
                x => x.AppointmentId == appointmentId, 
                includeProperties: "Items,Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            
            if (p == null)
            {
                var appt = await _appointmentRepo.GetFirstOrDefaultAsync(x => x.Id == appointmentId, includeProperties: "Patient.ApplicationUser,Doctor.ApplicationUser");
                if (appt == null) return null;

                return new PrescriptionViewModel
                {
                    AppointmentId = appointmentId,
                    PatientName = appt.Patient.ApplicationUser.FullName,
                    DoctorName = appt.Doctor.ApplicationUser.FullName
                };
            }

            return MapToViewModel(p);
        }

        public async Task<PrescriptionViewModel?> GetPrescriptionByIdAsync(int id)
        {
            var p = await _prescriptionRepo.GetFirstOrDefaultAsync(
                x => x.Id == id, 
                includeProperties: "Items,Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            
            if (p == null) return null;
            return MapToViewModel(p);
        }

        public async Task CreatePrescriptionAsync(PrescriptionViewModel model)
        {
            var prescription = new Prescription
            {
                AppointmentId = model.AppointmentId,
                Notes = model.Notes,
                CreatedAt = DateTime.UtcNow,
                Items = model.Items.Select(i => new PrescriptionItem
                {
                    MedicineName = i.MedicineName,
                    Dosage = i.Dosage,
                    Frequency = i.Frequency,
                    DurationDays = i.DurationDays,
                    SpecialInstructions = i.SpecialInstructions,
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };

            await _prescriptionRepo.AddAsync(prescription);
            await _prescriptionRepo.SaveAsync();
        }

        public async Task<IEnumerable<PrescriptionViewModel>> GetPatientPrescriptionsAsync(int patientId)
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync(
                filter: p => p.Appointment.PatientId == patientId,
                includeProperties: "Items,Appointment.Doctor.ApplicationUser");
            
            return prescriptions.Select(p => MapToViewModel(p));
        }

        private PrescriptionViewModel MapToViewModel(Prescription p)
        {
            return new PrescriptionViewModel
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                PatientName = p.Appointment?.Patient?.ApplicationUser?.FullName,
                DoctorName = p.Appointment?.Doctor?.ApplicationUser?.FullName,
                Notes = p.Notes,
                CreatedAt = p.CreatedAt,
                Items = p.Items.Select(i => new PrescriptionItemViewModel
                {
                    Id = i.Id,
                    MedicineName = i.MedicineName,
                    Dosage = i.Dosage,
                    Frequency = i.Frequency,
                    DurationDays = i.DurationDays,
                    SpecialInstructions = i.SpecialInstructions
                }).ToList()
            };
        }
    }
}
