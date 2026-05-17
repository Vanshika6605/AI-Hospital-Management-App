using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.ViewModels
{
    public class DoctorViewModel
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
        
        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        
        public string? DepartmentName { get; set; }
        
        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Qualification is required")]
        public string Qualifications { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Experience Years is required")]
        [Display(Name = "Experience (Years)")]
        public int ExperienceYears { get; set; }
        
        [Required(ErrorMessage = "Consultation Fee is required")]
        [Display(Name = "Consultation Fee")]
        public decimal ConsultationFees { get; set; }
        
        [Display(Name = "Availability Status")]
        public bool AvailabilityStatus { get; set; } = true;
        
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImage { get; set; }
        
        public string? ProfileImagePath { get; set; }
    }
}
