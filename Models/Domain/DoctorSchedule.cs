using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class DoctorSchedule : BaseEntity
    {
        [Required]
        public int DoctorId { get; set; }
        
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;
        
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public int SlotDurationMinutes { get; set; } = 30;
        
        public bool IsActive { get; set; } = true;
    }
}
