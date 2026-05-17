using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Prescription : BaseEntity
    {
        [Required]
        public int AppointmentId { get; set; }
        
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;
        
        public string? Notes { get; set; }

        public virtual ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
    }
}
