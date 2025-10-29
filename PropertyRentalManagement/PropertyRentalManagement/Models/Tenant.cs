using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        // Foreign Key linking to Users
        public int UserId { get; set; }

        [ValidateNever]
        public Users User { get; set; }

        // Foreign Key linking to PropertyOwner
        public int OwnerId { get; set; }

        [ValidateNever]
        public PropertyOwner Owner { get; set; }     


        // One-to-Many with Appointments
        public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();


    }
}
