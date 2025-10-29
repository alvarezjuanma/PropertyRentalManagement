using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Data
{
    public class PRMDbContext : DbContext
    {
        public PRMDbContext(DbContextOptions<PRMDbContext> options) : base(options)
        {
        }

        // DbSet properties for each entity
        public DbSet<Users> Users { get; set; }
        public DbSet<PropertyOwner> PropertyOwners { get; set; }
        public DbSet<PropertyManager> PropertyManagers { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<Apartments> Apartments { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<EventReport> EventReports { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de PropertyOwner y PropertyManager (NO ACTION para evitar cascada)
            modelBuilder.Entity<PropertyManager>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.PropertyManagers)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.NoAction); // No eliminación en cascada

            // Configuración de PropertyOwner y Tenant (NO ACTION para evitar cascada)
            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.Owner)
                .WithMany(o => o.Tenants)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.NoAction); // No eliminación en cascada

            // Configuración de PropertyManager y Buildings (NO ACTION para evitar cascada)
            modelBuilder.Entity<Buildings>()
                .HasOne(b => b.Manager)
                .WithMany(m => m.Buildings)
                .HasForeignKey(b => b.ManagerId)
                .OnDelete(DeleteBehavior.NoAction); // No eliminación en cascada

            // Configuración de PropertyManager y Apartments (NO ACTION para evitar cascada)
            modelBuilder.Entity<Apartments>()
                .HasOne(a => a.Manager)
                .WithMany(m => m.Apartments)
                .HasForeignKey(a => a.ManagerId)
                .OnDelete(DeleteBehavior.NoAction); // No eliminación en cascada

            // Configuración de Buildings y Apartments (Cascade para eliminar apartamentos al eliminar un edificio)
            modelBuilder.Entity<Apartments>()
                .HasOne(a => a.Building)
                .WithMany(b => b.Apartments)
                .HasForeignKey(a => a.BuildingId)
                .OnDelete(DeleteBehavior.Cascade); // Cascada permitida aquí           

            // Configuración de Tenant y Appointments (Cascade para eliminar citas al eliminar un inquilino)
            modelBuilder.Entity<Appointments>()
                .HasOne(ap => ap.Tenant)
                .WithMany(t => t.Appointments)
                .HasForeignKey(ap => ap.TenantId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un inquilino, se eliminan sus citas

            // Configuración de PropertyManager y Appointments (NO ACTION para evitar cascada)
            modelBuilder.Entity<Appointments>()
                .HasOne(ap => ap.Manager)
                .WithMany(m => m.Appointments)
                .HasForeignKey(ap => ap.ManagerId)
                .OnDelete(DeleteBehavior.NoAction); // No eliminación en cascada


            modelBuilder.Entity<Message>().HasData(
                new Message { MessageId = 1, Sender = "owner1@example.com", Recipient = "manager1@example.com", Content = "Hello, we need to discuss the lease agreement.", SentDate = DateTime.Now.AddDays(-5), TenantId = 1, ManagerId = 4, IsRead = false, Subject = "Lease Agreement" },
                new Message { MessageId = 2, Sender = "tenant1@example.com", Recipient = "manager1@example.com", Content = "I am having issues with the air conditioning. Could you help?", SentDate = DateTime.Now.AddDays(-3), TenantId = 1, ManagerId = 4, IsRead = true, Subject = "Air Conditioning Issue" },
                new Message { MessageId = 3, Sender = "manager1@example.com", Recipient = "owner1@example.com", Content = "The lease renewal is underway. I will keep you updated.", SentDate = DateTime.Now.AddDays(-2), TenantId = 1, ManagerId = 4, IsRead = false, Subject = "Contract Update" },
                new Message { MessageId = 4, Sender = "tenant1@example.com", Recipient = "manager1@example.com", Content = "Thanks for the help with the repair. Everything is working now.", SentDate = DateTime.Now.AddDays(-1), TenantId = 1, ManagerId = 4, IsRead = true, Subject = "Repair Completed" },
                new Message { MessageId = 5, Sender = "owner2@example.com", Recipient = "manager2@example.com", Content = "We need to review the financial reports for the last quarter.", SentDate = DateTime.Now, TenantId = 2, ManagerId = 5, IsRead = false, Subject = "Financial Reports" }
            );




            modelBuilder.Entity<Users>().HasData(
                new Users { UserId = 1, Username = "owner1", Password = "pass123", Email = "owner1@example.com", Phone = "555-555-5551", Role = UserRole.Owner },
                new Users { UserId = 2, Username = "manager1", Password = "pass123", Email = "manager1@example.com", Phone = "555-555-5552", Role = UserRole.Manager },
                new Users { UserId = 3, Username = "tenant1", Password = "pass123", Email = "tenant1@example.com", Phone = "555-555-5553", Role = UserRole.Tenant }
            );

            // Seed data for PropertyOwner
            modelBuilder.Entity<PropertyOwner>().HasData(
                new PropertyOwner { OwnerId = 1, Name = "John Doe", Email = "owner1@example.com", Phone = "555-555-5551", UserId = 1 } // Matches Users entry with UserId 1
            );

            // Seed data for PropertyManager
            modelBuilder.Entity<PropertyManager>().HasData(
                new PropertyManager { ManagerId = 1, Name = "Jane Smith", Email = "manager1@example.com", Phone = "555-555-5552", UserId = 2, OwnerId = 1 } // Matches Users entry with UserId 2 and OwnerId 1
            );

            // Seed data for Tenant
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant { TenantId = 1, Name = "Michael Brown", Email = "tenant1@example.com", Phone = "555-555-5553", UserId = 3, OwnerId = 1 } // Matches Users entry with UserId 3 and references OwnerId 1 and ManagerId 1
            );

            // Define initial seed data for Buildings
            modelBuilder.Entity<Buildings>().HasData(
                new Buildings { BuildingId = 1, Name = "Sunset Apartments", Address = "123 Main St", ManagerId = 1 },
                new Buildings { BuildingId = 2, Name = "Riverfront Plaza", Address = "456 River Ave", ManagerId = 1 },
                new Buildings { BuildingId = 3, Name = "Green Valley Towers", Address = "789 Valley Rd", ManagerId = 1 }
            );

            // Define initial seed data for Apartments
            modelBuilder.Entity<Apartments>().HasData(
                 new Apartments { ApartmentId = 1, ApartmentNumber = "101", Status = ApartmentStatus.Available, BuildingId = 1, ManagerId = 1, NumberOfBedrooms = 2, NumberOfBathrooms = 1, RentAmount = 1500, PetsAllowed = true },
                 new Apartments { ApartmentId = 2, ApartmentNumber = "102", Status = ApartmentStatus.Occupied, BuildingId = 1, ManagerId = 1, NumberOfBedrooms = 3, NumberOfBathrooms = 2, RentAmount = 2000, PetsAllowed = false },
                 new Apartments { ApartmentId = 3, ApartmentNumber = "201", Status = ApartmentStatus.Available, BuildingId = 2, ManagerId = 1, NumberOfBedrooms = 1, NumberOfBathrooms = 1, RentAmount = 1200, PetsAllowed = true },
                 new Apartments { ApartmentId = 4, ApartmentNumber = "202", Status = ApartmentStatus.InRepairing, BuildingId = 2, ManagerId = 1, NumberOfBedrooms = 2, NumberOfBathrooms = 2, RentAmount = 1800, PetsAllowed = false },
                 new Apartments { ApartmentId = 5, ApartmentNumber = "301", Status = ApartmentStatus.Available, BuildingId = 3, ManagerId = 1, NumberOfBedrooms = 4, NumberOfBathrooms = 3, RentAmount = 2500, PetsAllowed = true },
                 new Apartments { ApartmentId = 6, ApartmentNumber = "302", Status = ApartmentStatus.Occupied, BuildingId = 3, ManagerId = 1, NumberOfBedrooms = 2, NumberOfBathrooms = 1, RentAmount = 1600, PetsAllowed = false }
             );


            base.OnModelCreating(modelBuilder);
        }
    }
}
