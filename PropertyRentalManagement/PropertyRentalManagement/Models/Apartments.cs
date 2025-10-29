using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace PropertyRentalManagement.Models
{
    public class Apartments
    {
        [Key]
        public int ApartmentId { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(ApartmentStatus))]
        public ApartmentStatus Status { get; set; }

        [Display(Name = "Apartment Number")]
        [Required(ErrorMessage = "Apartment Number is required")]
        public string ApartmentNumber { get; set; } = string.Empty;

        // Foreign Key linking to Buildings
        public int BuildingId { get; set; }
        [ValidateNever]
        public Buildings Building { get; set; }

        // Foreign Key linking to PropertyManager
        public int ManagerId { get; set; }
        [ValidateNever]
        public PropertyManager Manager { get; set; }

        [Display(Name = "Number of Bedrooms")]
        [Range(1, 6, ErrorMessage = "Please enter a valid number of bedrooms.")]
        public int NumberOfBedrooms { get; set; }

        [Display(Name = "Number of Bathrooms")]
        [Range(1, 6, ErrorMessage = "Please enter a valid number of bathrooms.")]
        public int NumberOfBathrooms { get; set; }

        [Display(Name = "Rent Amount")]
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid rent amount.")]
        public int RentAmount { get; set; }

        [Display(Name = "Pets Allowed")]
        public bool PetsAllowed { get; set; }


        // Colección de Tenants (Relación One-to-Many)
        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
    public enum ApartmentStatus
    {
        Occupied,
        Available,
        InRepairing,  
        Pending
    }
}
