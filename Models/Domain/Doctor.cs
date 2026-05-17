using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Doctor : BaseEntity
    {
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        
        [Required]
        public int DepartmentId { get; set; }
        
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;
        
        public string Specialization { get; set; } = string.Empty;
        public string Qualifications { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal ConsultationFees { get; set; }
        public bool AvailabilityStatus { get; set; } = true;
        public string? ProfileImagePath { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }
}
