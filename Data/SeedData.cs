using AIHospitalManagementSys.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AIHospitalManagementSys.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine("--> Seeding Database...");
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();

            string[] roleNames = { "Admin", "Doctor", "Receptionist", "Patient" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    Console.WriteLine($"--> Creating Role: {roleName}");
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin user
            var adminEmail = "admin@hospital.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                Console.WriteLine($"--> Creating Admin User: {adminEmail}");
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    RoleName = "Admin",
                    EmailConfirmed = true
                };

                var createPowerUser = await userManager.CreateAsync(user, "Admin@123");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine("--> Admin User Created Successfully.");
                }
                else
                {
                    Console.WriteLine("--> Failed to create Admin User.");
                }
            }

            // Seed Departments
            if (!await context.Departments.AnyAsync())
            {
                Console.WriteLine("--> Seeding Departments...");
                var departments = new List<Department>
                {
                    new Department { Name = "Cardiology", Description = "Heart related treatments", CreatedAt = DateTime.UtcNow },
                    new Department { Name = "Neurology", Description = "Brain and nervous system", CreatedAt = DateTime.UtcNow },
                    new Department { Name = "Pediatrics", Description = "Child health care", CreatedAt = DateTime.UtcNow }
                };
                await context.Departments.AddRangeAsync(departments);
                await context.SaveChangesAsync();
            }

            // Seed a Doctor for testing
            var doctorEmail = "doctor@hospital.com";
            var existingDoctorUser = await userManager.FindByEmailAsync(doctorEmail);
            if (existingDoctorUser == null)
            {
                Console.WriteLine($"--> Creating Doctor User: {doctorEmail}");
                var docUser = new ApplicationUser
                {
                    UserName = doctorEmail,
                    Email = doctorEmail,
                    FullName = "Dr. John Doe",
                    RoleName = "Doctor",
                    EmailConfirmed = true
                };

                var createDoc = await userManager.CreateAsync(docUser, "Doctor@123");
                if (createDoc.Succeeded)
                {
                    await userManager.AddToRoleAsync(docUser, "Doctor");
                    
                    var cardiology = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Cardiology");
                    if (cardiology != null)
                    {
                        var doctor = new Doctor
                        {
                            ApplicationUserId = docUser.Id,
                            DepartmentId = cardiology.Id,
                            Specialization = "Cardiologist",
                            Qualifications = "MBBS, MD",
                            ConsultationFees = 500,
                            ExperienceYears = 10,
                            AvailabilityStatus = "Available",
                            CreatedAt = DateTime.UtcNow
                        };
                        await context.Doctors.AddAsync(doctor);
                        await context.SaveChangesAsync();
                        Console.WriteLine("--> Doctor Seeding Successful.");
                    }
                }
            }
            Console.WriteLine("--> Seeding Completed.");
        }
    }
}
