using System.ComponentModel.DataAnnotations;

namespace AIHospitalManagementSys.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
