using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class PropertyManager
    {
        [Key]
        public int ManagerId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = string.Empty;

        // Foreign Key linking to Users
        public int UserId { get; set; }

        [ValidateNever]
        public Users User { get; set; }

        // Foreign Key linking to PropertyOwner
        public int OwnerId { get; set; }

        [ValidateNever]
        public PropertyOwner Owner { get; set; }

        // One-to-Many with Buildings
        public virtual ICollection<Buildings> Buildings { get; set; } = new List<Buildings>();

        // One-to-Many with Apartments
        public virtual ICollection<Apartments> Apartments { get; set; } = new List<Apartments>();

        // One-to-Many with Tenants
        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

        // One-to-Many with Appointments 
        public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
    }
}
