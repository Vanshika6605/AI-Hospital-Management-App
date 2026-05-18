using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.ViewModels
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public int AppointmentId { get; set; }
        
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? Diagnosis { get; set; }
        
        public string? Notes { get; set; }
        
        public List<PrescriptionItemViewModel> Items { get; set; } = new List<PrescriptionItemViewModel>();
        
        public DateTime CreatedAt { get; set; }
    }

    public class PrescriptionItemViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Medicine")]
        public int MedicineId { get; set; }
        
        public string? MedicineName { get; set; } // For display
        
        [Required]
        public int Quantity { get; set; } = 1;
        
        [Required]
        public string Dosage { get; set; } = string.Empty;
        
        [Required]
        public string Frequency { get; set; } = string.Empty;
    }
}
