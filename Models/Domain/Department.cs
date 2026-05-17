using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Department : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
