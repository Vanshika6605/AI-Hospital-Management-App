using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class BillingService : IBillingService
    {
        private readonly IGenericRepository<Bill> _billRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;

        public BillingService(IGenericRepository<Bill> billRepo, IGenericRepository<Appointment> appointmentRepo)
        {
            _billRepo = billRepo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<IEnumerable<BillViewModel>> GetAllBillsAsync()
        {
            var bills = await _billRepo.GetAllAsync(includeProperties: "Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            return bills.Select(b => MapToViewModel(b));
        }

        public async Task<BillViewModel?> GetBillByIdAsync(int id)
        {
            var b = await _billRepo.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            if (b == null) return null;
            return MapToViewModel(b);
        }

        public async Task<BillViewModel?> GetBillByAppointmentIdAsync(int appointmentId)
        {
            var b = await _billRepo.GetFirstOrDefaultAsync(x => x.AppointmentId == appointmentId, includeProperties: "Appointment.Patient.ApplicationUser,Appointment.Doctor.ApplicationUser");
            if (b == null)
            {
                var appt = await _appointmentRepo.GetFirstOrDefaultAsync(x => x.Id == appointmentId, includeProperties: "Patient.ApplicationUser,Doctor.ApplicationUser");
                if (appt == null) return null;
                
                return new BillViewModel
                {
                    AppointmentId = appointmentId,
                    PatientName = appt.Patient.ApplicationUser.FullName,
                    DoctorName = appt.Doctor.ApplicationUser.FullName,
                    AppointmentDate = appt.AppointmentDate,
                    ConsultationCharges = appt.Doctor.ConsultationFees
                };
            }
            return MapToViewModel(b);
        }

        public async Task GenerateBillAsync(BillViewModel model)
        {
            var bill = new Bill
            {
                AppointmentId = model.AppointmentId,
                ConsultationCharges = model.ConsultationCharges,
                MedicineCharges = model.MedicineCharges,
                LabCharges = model.LabCharges,
                ServiceCharges = model.ServiceCharges,
                TotalAmount = model.TotalAmount,
                PaymentStatus = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _billRepo.AddAsync(bill);
            await _billRepo.SaveAsync();
        }

        public async Task UpdatePaymentStatusAsync(int id, string status)
        {
            var bill = await _billRepo.GetByIdAsync(id);
            if (bill != null)
            {
                bill.PaymentStatus = status;
                bill.UpdatedAt = DateTime.UtcNow;
                _billRepo.Update(bill);
                await _billRepo.SaveAsync();
            }
        }

        private BillViewModel MapToViewModel(Bill b)
        {
            return new BillViewModel
            {
                Id = b.Id,
                AppointmentId = b.AppointmentId,
                PatientName = b.Appointment?.Patient?.ApplicationUser?.FullName,
                DoctorName = b.Appointment?.Doctor?.ApplicationUser?.FullName,
                AppointmentDate = b.Appointment?.AppointmentDate ?? DateTime.MinValue,
                ConsultationCharges = b.ConsultationCharges,
                MedicineCharges = b.MedicineCharges,
                LabCharges = b.LabCharges,
                ServiceCharges = b.ServiceCharges,
                PaymentStatus = b.PaymentStatus,
                CreatedAt = b.CreatedAt
            };
        }
    }
}
