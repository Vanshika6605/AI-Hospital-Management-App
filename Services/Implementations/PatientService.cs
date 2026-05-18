using AIHospitalManagementSys.Models.Domain;
using AIHospitalManagementSys.Repositories.Interfaces;
using AIHospitalManagementSys.Services.Interfaces;
using AIHospitalManagementSys.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IGenericRepository<Patient> _patientRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(IGenericRepository<Patient> patientRepo, UserManager<ApplicationUser> userManager)
        {
            _patientRepo = patientRepo;
            _userManager = userManager;
        }

        public async Task<IEnumerable<PatientViewModel>> GetAllPatientsAsync(string? search = null)
        {
            var patients = await _patientRepo.GetAllAsync(
                filter: p => string.IsNullOrEmpty(search) || p.ApplicationUser.FullName.Contains(search) || p.ContactNumber.Contains(search),
                includeProperties: "ApplicationUser");

            return patients.Select(p => new PatientViewModel
            {
                Id = p.Id,
                ApplicationUserId = p.ApplicationUserId,
                FullName = p.ApplicationUser?.FullName ?? "Unknown",
                Email = p.ApplicationUser?.Email ?? "",
                ContactNumber = p.ContactNumber,
                BloodGroup = p.BloodGroup,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth
            });
        }

        public async Task<PatientViewModel?> GetPatientByIdAsync(int id)
        {
            var p = await _patientRepo.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "ApplicationUser");
            if (p == null) return null;

            return new PatientViewModel
            {
                Id = p.Id,
                ApplicationUserId = p.ApplicationUserId,
                FullName = p.ApplicationUser?.FullName ?? "Unknown",
                Email = p.ApplicationUser?.Email ?? "",
                ContactNumber = p.ContactNumber,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                BloodGroup = p.BloodGroup,
                Address = p.Address,
                EmergencyContact = p.EmergencyContact,
                Allergies = p.Allergies,
                ExistingConditions = p.ExistingConditions
            };
        }

        public async Task CreatePatientAsync(PatientViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                RoleName = "Patient",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password ?? "Patient@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");

                var patient = new Patient
                {
                    ApplicationUserId = user.Id,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    BloodGroup = model.BloodGroup,
                    ContactNumber = model.ContactNumber,
                    Address = model.Address,
                    EmergencyContact = model.EmergencyContact,
                    Allergies = model.Allergies,
                    ExistingConditions = model.ExistingConditions,
                    CreatedAt = DateTime.UtcNow
                };

                await _patientRepo.AddAsync(patient);
                await _patientRepo.SaveAsync();
            }
            else
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task UpdatePatientAsync(PatientViewModel model)
        {
            var patient = await _patientRepo.GetFirstOrDefaultAsync(x => x.Id == model.Id, includeProperties: "ApplicationUser");
            if (patient != null)
            {
                patient.DateOfBirth = model.DateOfBirth;
                patient.Gender = model.Gender;
                patient.BloodGroup = model.BloodGroup;
                patient.ContactNumber = model.ContactNumber;
                patient.Address = model.Address;
                patient.EmergencyContact = model.EmergencyContact;
                patient.Allergies = model.Allergies;
                patient.ExistingConditions = model.ExistingConditions;
                patient.UpdatedAt = DateTime.UtcNow;

                patient.ApplicationUser.FullName = model.FullName;
                patient.ApplicationUser.Email = model.Email;
                patient.ApplicationUser.UserName = model.Email;

                await _userManager.UpdateAsync(patient.ApplicationUser);
                _patientRepo.Update(patient);
                await _patientRepo.SaveAsync();
            }
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient != null)
            {
                var user = await _userManager.FindByIdAsync(patient.ApplicationUserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
                _patientRepo.Remove(patient);
                await _patientRepo.SaveAsync();
            }
        }
    }
}
