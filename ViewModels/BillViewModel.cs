using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.ViewModels
{
    public class BillViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public int AppointmentId { get; set; }
        
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        
        [Required]
        [Display(Name = "Consultation Charges")]
        public decimal ConsultationCharges { get; set; }
        
        [Display(Name = "Medicine Charges")]
        public decimal MedicineCharges { get; set; }
        
        [Display(Name = "Lab Charges")]
        public decimal LabCharges { get; set; }
        
        [Display(Name = "Service Charges")]
        public decimal ServiceCharges { get; set; }
        
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => ConsultationCharges + MedicineCharges + LabCharges + ServiceCharges;
        
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; }
    }
}
