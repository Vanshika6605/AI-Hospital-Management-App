using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        
        public string? ApplicationUserId { get; set; }
        
        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Blood Group is required")]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Contact Number is required")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Emergency Contact is required")]
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;
        
        public string? Allergies { get; set; }
        
        [Display(Name = "Existing Conditions")]
        public string? ExistingConditions { get; set; }
    }
}
