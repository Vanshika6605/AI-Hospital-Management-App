using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class Notification : BaseEntity
    {
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        public bool IsRead { get; set; } = false;
    }
}
