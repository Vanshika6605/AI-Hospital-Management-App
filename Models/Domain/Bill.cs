using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Bill : BaseEntity
    {
        [Required]
        public int AppointmentId { get; set; }
        
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;
        
        public decimal ConsultationCharges { get; set; }
        public decimal MedicineCharges { get; set; }
        public decimal LabCharges { get; set; }
        public decimal ServiceCharges { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public string PaymentStatus { get; set; } = "Pending";
    }
}
