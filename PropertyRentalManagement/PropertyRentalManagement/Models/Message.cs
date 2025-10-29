using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [Display(Name = "Sender")]
        public string Sender { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [Required]
        [Display(Name = "Message Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Sent Date")]
        public DateTime SentDate { get; set; }

        public bool IsRead { get; set; } = false;

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        // Foreign Key to Tenant (if the message is from a Tenant)
        public int? TenantId { get; set; }
        [ValidateNever]
        public Tenant Tenant { get; set; }

        // Foreign Key to Property Manager (if the message is from a Property Manager)
        public int? ManagerId { get; set; }
        [ValidateNever]
        public PropertyManager Manager { get; set; }
    }
}
