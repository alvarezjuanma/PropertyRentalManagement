using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class Buildings
    {
        [Key]
        public int BuildingId { get; set; }

        [Display(Name = "Building Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        // Foreign Key linking to PropertyManager
        public int? ManagerId { get; set; }
        [ValidateNever]
        public PropertyManager? Manager { get; set; }

        // One-to-Many with Apartments
        public virtual ICollection<Apartments> Apartments { get; set; } = new List<Apartments>();
    }
}