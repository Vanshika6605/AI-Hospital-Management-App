using AIHospitalManagementSys.ViewModels;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<BillViewModel>> GetAllBillsAsync();
        Task<BillViewModel?> GetBillByIdAsync(int id);
        Task<BillViewModel?> GetBillByAppointmentIdAsync(int appointmentId);
        Task GenerateBillAsync(BillViewModel model);
        Task UpdatePaymentStatusAsync(int id, string status);
    }
}
