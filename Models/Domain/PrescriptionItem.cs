using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHospitalManagementSys.Models.Domain
{
    public class PrescriptionItem : BaseEntity
    {
        [Required]
        public int PrescriptionId { get; set; }
        
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; } = null!;
        
        [Required]
        public int MedicineId { get; set; }
        
        [ForeignKey("MedicineId")]
        public virtual MedicineCatalog Medicine { get; set; } = null!;

        [Required]
        public int Quantity { get; set; } = 1;
        
        public string Dosage { get; set; } = string.Empty; // e.g. 1-0-1
        public string Frequency { get; set; } = string.Empty; // e.g. Daily
    }
}
