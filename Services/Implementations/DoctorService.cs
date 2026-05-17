using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DoctorService(IGenericRepository<Doctor> doctorRepo, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _doctorRepo = doctorRepo;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAllDoctorsAsync(string? search = null, int? departmentId = null)
        {
            var doctors = await _doctorRepo.GetAllAsync(
                filter: d => (string.IsNullOrEmpty(search) || d.ApplicationUser.FullName.Contains(search) || d.Specialization.Contains(search))
                            && (!departmentId.HasValue || d.DepartmentId == departmentId.Value),
                includeProperties: "ApplicationUser,Department");

            return doctors.Select(d => new DoctorViewModel
            {
                Id = d.Id,
                ApplicationUserId = d.ApplicationUserId,
                FullName = d.ApplicationUser.FullName,
                Email = d.ApplicationUser.Email!,
                DepartmentId = d.DepartmentId,
                DepartmentName = d.Department.Name,
                Specialization = d.Specialization,
                ConsultationFees = d.ConsultationFees,
                ExperienceYears = d.ExperienceYears,
                AvailabilityStatus = d.AvailabilityStatus,
                ProfileImagePath = d.ProfileImagePath
            });
        }

        public async Task<DoctorViewModel?> GetDoctorByIdAsync(int id)
        {
            var d = await _doctorRepo.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "ApplicationUser,Department");
            if (d == null) return null;

            return new DoctorViewModel
            {
                Id = d.Id,
                ApplicationUserId = d.ApplicationUserId,
                FullName = d.ApplicationUser.FullName,
                Email = d.ApplicationUser.Email!,
                DepartmentId = d.DepartmentId,
                DepartmentName = d.Department.Name,
                Specialization = d.Specialization,
                Qualifications = d.Qualifications,
                ExperienceYears = d.ExperienceYears,
                ConsultationFees = d.ConsultationFees,
                AvailabilityStatus = d.AvailabilityStatus,
                PhoneNumber = d.ApplicationUser.PhoneNumber,
                ProfileImagePath = d.ProfileImagePath
            };
        }

        public async Task CreateDoctorAsync(DoctorViewModel model)
        {
            string? fileName = await UploadFile(model.ProfileImage);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                RoleName = "Doctor",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password ?? "Doctor@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");

                var doctor = new Doctor
                {
                    ApplicationUserId = user.Id,
                    DepartmentId = model.DepartmentId,
                    Specialization = model.Specialization,
                    Qualifications = model.Qualifications,
                    ExperienceYears = model.ExperienceYears,
                    ConsultationFees = model.ConsultationFees,
                    AvailabilityStatus = model.AvailabilityStatus,
                    ProfileImagePath = fileName,
                    CreatedAt = DateTime.UtcNow
                };

                await _doctorRepo.AddAsync(doctor);
                await _doctorRepo.SaveAsync();
            }
            else
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task UpdateDoctorAsync(DoctorViewModel model)
        {
            var doctor = await _doctorRepo.GetFirstOrDefaultAsync(x => x.Id == model.Id, includeProperties: "ApplicationUser");
            if (doctor != null)
            {
                if (model.ProfileImage != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(doctor.ProfileImagePath))
                    {
                        DeleteFile(doctor.ProfileImagePath);
                    }
                    doctor.ProfileImagePath = await UploadFile(model.ProfileImage);
                }

                doctor.DepartmentId = model.DepartmentId;
                doctor.Specialization = model.Specialization;
                doctor.Qualifications = model.Qualifications;
                doctor.ExperienceYears = model.ExperienceYears;
                doctor.ConsultationFees = model.ConsultationFees;
                doctor.AvailabilityStatus = model.AvailabilityStatus;
                doctor.UpdatedAt = DateTime.UtcNow;

                doctor.ApplicationUser.FullName = model.FullName;
                doctor.ApplicationUser.PhoneNumber = model.PhoneNumber;
                doctor.ApplicationUser.Email = model.Email;
                doctor.ApplicationUser.UserName = model.Email;

                await _userManager.UpdateAsync(doctor.ApplicationUser);
                _doctorRepo.Update(doctor);
                await _doctorRepo.SaveAsync();
            }
        }

        private async Task<string?> UploadFile(IFormFile? file)
        {
            if (file == null) return null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "doctors");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/doctors/" + uniqueFileName;
        }

        private void DeleteFile(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor != null)
            {
                var user = await _userManager.FindByIdAsync(doctor.ApplicationUserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
                _doctorRepo.Remove(doctor);
                await _doctorRepo.SaveAsync();
            }
        }
    }
}
