using Microsoft.AspNetCore.Mvc;
using PropertyRentalManagement.Data;
using PropertyRentalManagement.Models;
using Microsoft.EntityFrameworkCore;



namespace PropertyRentalManagement.Controllers
{
    public class EventReportsController : Controller
    {
        private readonly PRMDbContext _context;

        public EventReportsController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: EventReports
        public async Task<IActionResult> Index()
        {
           
            var managerIdString = HttpContext.Session.GetString("ManagerId");
            var ownerIdString = HttpContext.Session.GetString("OwnerId");

            IQueryable<EventReport> reports;

            if (!string.IsNullOrEmpty(ownerIdString))
            {
                
                reports = _context.EventReports
                    .Include(r => r.Manager)
                    .Include(r => r.Tenant); 
            }
            else if (!string.IsNullOrEmpty(managerIdString) && int.TryParse(managerIdString, out int managerId))
            {
                
                reports = _context.EventReports
                    .Where(r => r.ManagerId == managerId)
                    .Include(r => r.Manager)
                    .Include(r => r.Tenant);
            }
            else
            {
                
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }


            var reportList = await reports.OrderByDescending(r => r.ReportedDate).ToListAsync();
            return View(reportList);
            
        }

        // GET: EventReports/Create
        public async Task<IActionResult> Create()
        {
            var managerId = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerId))
            {
                TempData["ErrorMessage"] = "Manager session expired or not set. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            
            ViewBag.Tenants = await _context.Tenants
                .Select(t => new { t.TenantId, t.Name })
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenantId,Title,Description,Status")] EventReport eventReport)
        {
            
            var managerIdString = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerIdString) || !int.TryParse(managerIdString, out int managerId))
            {
                ModelState.AddModelError("ManagerId", "Manager session expired or not set. Please log in again.");
                ViewBag.Tenants = await _context.Tenants
                    .Select(t => new { t.TenantId, t.Name })
                    .ToListAsync();
                return View(eventReport);
            }

            
            eventReport.ManagerId = managerId;
            eventReport.ReportedDate = DateTime.Now;

           
            if (eventReport.TenantId.HasValue)
            {
                var tenant = await _context.Tenants.FindAsync(eventReport.TenantId.Value);
                if (tenant == null)
                {
                    ModelState.AddModelError("TenantId", "Invalid tenant selected.");
                    ViewBag.Tenants = await _context.Tenants
                        .Select(t => new { t.TenantId, t.Name })
                        .ToListAsync();
                    return View(eventReport);
                }
            }

            if (ModelState.IsValid)
            {
                _context.EventReports.Add(eventReport);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Report created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Tenants = await _context.Tenants
                .Select(t => new { t.TenantId, t.Name })
                .ToListAsync();
            return View(eventReport);
        }
    }

}


