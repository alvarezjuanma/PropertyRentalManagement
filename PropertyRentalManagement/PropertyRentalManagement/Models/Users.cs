using System.ComponentModel.DataAnnotations;

namespace PropertyRentalManagement.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Role")]
        public UserRole Role { get; set; }
        
    }

    public enum UserRole
    {
        Owner,
        Manager,
        Tenant
    }
}
