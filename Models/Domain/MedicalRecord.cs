using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class MedicalRecord : BaseEntity
    {
        [Required]
        public int AppointmentId { get; set; }
        
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;
        
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public string FileType { get; set; } = string.Empty;
        
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        
        public string? Description { get; set; }
    }
}
