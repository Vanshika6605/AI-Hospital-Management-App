using AIHospitalManagementSys.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIHospitalManagementSys.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            
            // Appointment - Prescription (1:1)
            builder.Entity<Appointment>()
                .HasOne(a => a.Prescription)
                .WithOne(p => p.Appointment)
                .HasForeignKey<Prescription>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment - Bill (1:1)
            builder.Entity<Appointment>()
                .HasOne(a => a.Bill)
                .WithOne(b => b.Appointment)
                .HasForeignKey<Bill>(b => b.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment relationships to avoid multiple cascade paths
            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Precision for decimal fields
            builder.Entity<Doctor>()
                .Property(d => d.ConsultationFees)
                .HasPrecision(18, 2);

            builder.Entity<Bill>()
                .Property(b => b.ConsultationCharges).HasPrecision(18, 2);
            builder.Entity<Bill>()
                .Property(b => b.MedicineCharges).HasPrecision(18, 2);
            builder.Entity<Bill>()
                .Property(b => b.LabCharges).HasPrecision(18, 2);
            builder.Entity<Bill>()
                .Property(b => b.ServiceCharges).HasPrecision(18, 2);
            builder.Entity<Bill>()
                .Property(b => b.TotalAmount).HasPrecision(18, 2);
        }
    }
}
