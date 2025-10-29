using Microsoft.AspNetCore.Mvc;
using PropertyRentalManagement.Data;
using PropertyRentalManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace PropertyRentalManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly PRMDbContext _context;

        public AccountController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Verificar si el usuario existe en la base de datos
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }


            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", user.Role.ToString());

            if (user.Role == UserRole.Owner)
            {
                var owner = await _context.PropertyOwners.FirstOrDefaultAsync(o => o.UserId == user.UserId);
                if (owner != null)
                {
                    HttpContext.Session.SetString("OwnerId", owner.OwnerId.ToString());
                }
                else
                {
                    ViewBag.ErrorMessage = "Owner not found.";
                    return View();
                }
            }
            else if (user.Role == UserRole.Manager)
            {
                var manager = await _context.PropertyManagers.FirstOrDefaultAsync(m => m.UserId == user.UserId);
                if (manager != null)
                {
                    HttpContext.Session.SetString("ManagerId", manager.ManagerId.ToString());
                }
                else
                {
                    ViewBag.ErrorMessage = "Manager not found.";
                    return View();
                }
            }
            else if (user.Role == UserRole.Tenant)
            {
                var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == user.UserId);
                if (tenant != null)
                {
                    HttpContext.Session.SetString("TenantId", tenant.TenantId.ToString());
                }
                else
                {
                    ViewBag.ErrorMessage = "Tenant not found.";
                    return View();
                }
            }

            Console.WriteLine($"UserId: {HttpContext.Session.GetString("UserId")}");
            Console.WriteLine($"OwnerId: {HttpContext.Session.GetString("OwnerId")}");
            Console.WriteLine($"ManagerId: {HttpContext.Session.GetString("ManagerId")}");
            Console.WriteLine($"TenantId: {HttpContext.Session.GetString("TenantId")}");
            Console.WriteLine($"Role: {HttpContext.Session.GetString("Role")}");

            switch (user.Role)
            {
                case UserRole.Owner: // Owner
                    return RedirectToAction("OwnerDashboard", "PropertyOwners", new { id = user.UserId });

                case UserRole.Manager: // Manager
                    return RedirectToAction("ManagerDashboard", "PropertyManagers", new { id = user.UserId });

                case UserRole.Tenant: // Tenant
                    return RedirectToAction("TenantDashboard", "Tenants", new { id = user.UserId });

                default:
                    ViewBag.ErrorMessage = "User role is not valid.";
                    return View();
            }
            
        }        

            // Logout method to clear the session
        public IActionResult Logout()
        {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
        }

        public IActionResult CheckSession()
        {
            var ownerId = HttpContext.Session.GetString("OwnerId");
            Console.WriteLine($"OwnerId: {ownerId}");
            return View();
        }
    }
}
