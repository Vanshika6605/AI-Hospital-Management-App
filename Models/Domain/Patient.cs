using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Patient : BaseEntity
    {
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string? Allergies { get; set; }
        public string? ExistingConditions { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
