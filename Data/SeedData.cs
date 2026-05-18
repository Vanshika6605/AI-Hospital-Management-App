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

            // Seed Medicine Catalog
            if (!await context.MedicineCatalogs.AnyAsync())
            {
                Console.WriteLine("--> Seeding Medicine Catalog...");
                var medicines = new List<MedicineCatalog>
                {
                    new MedicineCatalog { MedicineName = "Paracetamol 500mg", Price = 5.00m, Stock = 1000, Description = "Fever and mild pain relief", CreatedAt = DateTime.UtcNow },
                    new MedicineCatalog { MedicineName = "Amoxicillin 250mg", Price = 15.50m, Stock = 500, Description = "Antibiotic", CreatedAt = DateTime.UtcNow },
                    new MedicineCatalog { MedicineName = "Ibuprofen 400mg", Price = 8.00m, Stock = 800, Description = "Anti-inflammatory", CreatedAt = DateTime.UtcNow },
                    new MedicineCatalog { MedicineName = "Cetirizine 10mg", Price = 4.00m, Stock = 600, Description = "Allergy relief", CreatedAt = DateTime.UtcNow },
                    new MedicineCatalog { MedicineName = "Omeprazole 20mg", Price = 12.00m, Stock = 400, Description = "Acid reflux", CreatedAt = DateTime.UtcNow }
                };
                await context.MedicineCatalogs.AddRangeAsync(medicines);
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
                            AvailabilityStatus = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        await context.Doctors.AddAsync(doctor);
                        await context.SaveChangesAsync();
                        Console.WriteLine("--> Doctor Seeding Successful.");
                    }
                }
            }
            // Seed a Receptionist (Desk)
            var deskEmail = "desk@hospital.com";
            if (await userManager.FindByEmailAsync(deskEmail) == null)
            {
                Console.WriteLine($"--> Creating Desk User: {deskEmail}");
                var deskUser = new ApplicationUser
                {
                    UserName = deskEmail,
                    Email = deskEmail,
                    FullName = "Front Desk",
                    RoleName = "Receptionist",
                    EmailConfirmed = true
                };
                if ((await userManager.CreateAsync(deskUser, "Desk@123")).Succeeded)
                {
                    await userManager.AddToRoleAsync(deskUser, "Receptionist");
                }
            }

            // Seed a Patient
            var patientEmail = "patient@hospital.com";
            if (await userManager.FindByEmailAsync(patientEmail) == null)
            {
                Console.WriteLine($"--> Creating Patient User: {patientEmail}");
                var patientUser = new ApplicationUser
                {
                    UserName = patientEmail,
                    Email = patientEmail,
                    FullName = "Jane Doe",
                    RoleName = "Patient",
                    EmailConfirmed = true
                };
                if ((await userManager.CreateAsync(patientUser, "Patient@123")).Succeeded)
                {
                    await userManager.AddToRoleAsync(patientUser, "Patient");
                    var patientDomain = new AIHospitalManagementSys.Models.Domain.Patient
                    {
                        ApplicationUserId = patientUser.Id,
                        DateOfBirth = DateTime.UtcNow.AddYears(-25),
                        CreatedAt = DateTime.UtcNow
                    };
                    await context.Patients.AddAsync(patientDomain);
                    await context.SaveChangesAsync();
                }
            }

            Console.WriteLine("--> Seeding Completed.");
        }
    }
}
