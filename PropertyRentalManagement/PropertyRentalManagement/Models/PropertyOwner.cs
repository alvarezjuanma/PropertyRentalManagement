using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class PropertyOwner
    {
        [Key]
        public int OwnerId { get; set; }

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

        // One-to-Many with PropertyManagers 
        public virtual ICollection<PropertyManager> PropertyManagers { get; set; } = new List<PropertyManager>();

        // One-to-Many with Tenants 
        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();


    }
}
