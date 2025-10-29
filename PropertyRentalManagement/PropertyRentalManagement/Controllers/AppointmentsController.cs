using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Data;
using PropertyRentalManagement.Models;


namespace PropertyRentalManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly PRMDbContext _context;

        public AppointmentsController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var pRMDbContext = _context.Appointments.Include(a => a.Manager).Include(a => a.Tenant);
            return View(await pRMDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Manager)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointments == null)
            {
                return NotFound();
            }

            return View(appointments);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Name");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name");
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "ApartmentId", "ApartmentNumber"); 
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime date, string status, Appointments model)
        {
            int tenantId = 0;
            int managerId = 0;

            // Obtener TenantId y ManagerId desde la sesión
            var tenantIdString = HttpContext.Session.GetString("TenantId");
            if (string.IsNullOrEmpty(tenantIdString) || !int.TryParse(tenantIdString, out tenantId))
            {
                ModelState.AddModelError("TenantId", "Tenant is not logged in or TenantId is invalid.");
                ViewBag.ErrorMessage = "Tenant ID is missing or invalid. Please log in as a Tenant.";
                Console.WriteLine("Error: TenantId is missing or invalid. Check session and login.");
            }
            else
            {
                model.TenantId = tenantId;
                Console.WriteLine($"TenantId is valid: {tenantId}");
            }

            var managerIdString = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerIdString) || !int.TryParse(managerIdString, out managerId))
            {
                ModelState.AddModelError("ManagerId", "Manager is not logged in or ManagerId is invalid.");
                ViewBag.ErrorMessage = "Manager ID is missing or invalid. Please log in as a Manager.";
                Console.WriteLine("Error: ManagerId is missing or invalid. Check session and login.");
            }
            else
            {
                model.ManagerId = managerId;
                Console.WriteLine($"ManagerId is valid: {managerId}");
            }

            // Validar ApartmentId
            if (model.ApartmentId <= 0)
            {
                ModelState.AddModelError("ApartmentId", "Apartment ID is required and must be valid.");
                ViewBag.ErrorMessage = "Apartment ID is missing or invalid. Please select a valid Apartment.";
                Console.WriteLine("Error: ApartmentId is missing or invalid.");
            }

            // Verificación de existencia en la base de datos
            var tenantExists = _context.Tenants.Any(t => t.TenantId == tenantId);
            var managerExists = _context.PropertyManagers.Any(m => m.ManagerId == managerId);
            var apartmentExists = _context.Apartments.Any(a => a.ApartmentId == model.ApartmentId);

            if (!tenantExists)
            {
                ModelState.AddModelError("TenantId", "The specified Tenant does not exist.");
                Console.WriteLine("Error: The specified TenantId does not exist in the database.");
            }

            if (!managerExists)
            {
                ModelState.AddModelError("ManagerId", "The specified Manager does not exist.");
                Console.WriteLine("Error: The specified ManagerId does not exist in the database.");
            }

            if (!apartmentExists)
            {
                ModelState.AddModelError("ApartmentId", "The specified Apartment does not exist.");
                Console.WriteLine("Error: The specified ApartmentId does not exist in the database.");
            }

            try
            {
                
                model.Date = date != DateTime.MinValue ? date : DateTime.Now;
                model.Status = !string.IsNullOrEmpty(status) ? status : "Scheduled";

               
                _context.Appointments.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }



        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId", appointments.ManagerId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", appointments.TenantId);
            return View(appointments);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,Date,Status,TenantId,ManagerId")] Appointments appointments)
        {
            if (id != appointments.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentsExists(appointments.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId", appointments.ManagerId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "TenantId", appointments.TenantId);
            return View(appointments);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Manager)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointments == null)
            {
                return NotFound();
            }

            return View(appointments);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointments = await _context.Appointments.FindAsync(id);
            if (appointments != null)
            {
                _context.Appointments.Remove(appointments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentsExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
