using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;

using AIHospitalManagementSys.Data;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IGenericRepository<Prescription> _prescriptionRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<MedicineCatalog> _medicineRepo;
        private readonly IBillingService _billingService;
        private readonly ApplicationDbContext _context;

        public PrescriptionService(
            IGenericRepository<Prescription> prescriptionRepo, 
            IGenericRepository<Appointment> appointmentRepo,
            IGenericRepository<MedicineCatalog> medicineRepo,
            IBillingService billingService,
            ApplicationDbContext context)
        {
            _prescriptionRepo = prescriptionRepo;
            _appointmentRepo = appointmentRepo;
            _medicineRepo = medicineRepo;
            _billingService = billingService;
            _context = context;
        }

        public async Task<PrescriptionViewModel?> GetPrescriptionByAppointmentIdAsync(int appointmentId)
        {
            var p = await _prescriptionRepo.GetFirstOrDefaultAsync(
                x => x.AppointmentId == appointmentId, 
                includeProperties: "Items.Medicine,Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            
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
                includeProperties: "Items.Medicine,Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            
            if (p == null) return null;
            return MapToViewModel(p);
        }

        public async Task CreatePrescriptionAsync(PrescriptionViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var prescription = new Prescription
                {
                    AppointmentId = model.AppointmentId,
                    Notes = model.Notes,
                    CreatedAt = DateTime.UtcNow,
                    Items = model.Items.Select(i => new PrescriptionItem
                    {
                        MedicineId = i.MedicineId,
                        Quantity = i.Quantity,
                        Dosage = i.Dosage,
                        Frequency = i.Frequency,
                        CreatedAt = DateTime.UtcNow
                    }).ToList()
                };

                await _prescriptionRepo.AddAsync(prescription);
                await _prescriptionRepo.SaveAsync();

                // Calculate Medicine Charges
                decimal medicineCharges = 0;
                foreach (var item in model.Items)
                {
                    var medicine = await _medicineRepo.GetFirstOrDefaultAsync(m => m.Id == item.MedicineId);
                    if (medicine != null)
                    {
                        medicineCharges += medicine.Price * item.Quantity;
                    }
                }

                // Generate Draft Bill
                var bill = new BillViewModel
                {
                    AppointmentId = model.AppointmentId,
                    MedicineCharges = medicineCharges,
                    ConsultationCharges = 500, // Hardcoded for now, could fetch from Doctor
                    LabCharges = 0,
                    ServiceCharges = 50,
                    PaymentStatus = "Draft",
                    CreatedAt = DateTime.UtcNow
                };

                await _billingService.GenerateBillAsync(bill);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<PrescriptionViewModel>> GetPatientPrescriptionsAsync(int patientId)
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync(
                filter: p => p.Appointment.PatientId == patientId,
                includeProperties: "Items.Medicine,Appointment.Doctor.ApplicationUser");
            
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
                    MedicineId = i.MedicineId,
                    MedicineName = i.Medicine?.MedicineName,
                    Quantity = i.Quantity,
                    Dosage = i.Dosage,
                    Frequency = i.Frequency
                }).ToList()
            };
        }
    }
}
