using System.ComponentModel.DataAnnotations;
using AIHospitalManagementSys.Models.Enums;

namespace AIHospitalManagementSys.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Doctor selection is required")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        
        public string? DoctorName { get; set; }
        public string? DepartmentName { get; set; }
        
        [Required(ErrorMessage = "Patient selection is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        
        public string? PatientName { get; set; }
        
        [Required(ErrorMessage = "Appointment Date and Time is required")]
        [Display(Name = "Appointment Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        
        public DateTime CreatedAt { get; set; }
    }
}
