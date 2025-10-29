using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class EventReport
    {
        [Key]
        public int EventReportId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Reported Date")]
        public DateTime ReportedDate { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending"; // Default status is "Pending"

        // Foreign Key linking to PropertyManager
        public int ManagerId { get; set; }
        [ValidateNever]
        public PropertyManager Manager { get; set; }

        // Optional Foreign Key linking to Tenant, if needed
        public int? TenantId { get; set; }
        [ValidateNever]
        public Tenant? Tenant { get; set; }
    }
}
