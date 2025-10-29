using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class Appointments
    {
        [Key]
        public int AppointmentId { get; set; }

        [Display(Name = "Appointment Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;  // Scheduled, Completed, Canceled

        // Foreign Key linking to Tenant
        public int TenantId { get; set; }
        [ValidateNever]
        public Tenant Tenant { get; set; }

        // Foreign Key linking to PropertyManager
        public int ManagerId { get; set; }
        [ValidateNever]
        public PropertyManager Manager { get; set; }

        // Foreign Key linking to Apartment
        [Display(Name = "Apartment")]
        public int ApartmentId { get; set; }  // Relacionando la cita con un apartamento específico
        [ValidateNever]
        public Apartments Apartment { get; set; }  // Relación con el modelo Apartments
    }
}
