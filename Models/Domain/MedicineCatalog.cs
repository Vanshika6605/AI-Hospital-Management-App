using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class MedicineCatalog : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string MedicineName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; } = 0;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
