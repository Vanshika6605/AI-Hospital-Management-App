using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AIHospitalManagementSys.Models.Enums;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Appointment : BaseEntity
    {
        [Required]
        public int DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;
        
        [Required]
        public int PatientId { get; set; }
        
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        public string? Reason { get; set; }
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // Navigation properties
        public virtual Prescription? Prescription { get; set; }
        public virtual Bill? Bill { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
