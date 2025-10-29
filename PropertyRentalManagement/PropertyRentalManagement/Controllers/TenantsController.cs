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
    public class TenantsController : Controller
    {
        private readonly PRMDbContext _context;

        public TenantsController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: Tenants
        public async Task<IActionResult> Index()
        {
            var pRMDbContext = _context.Tenants.Include(t => t.Owner).Include(t => t.User);
            return View(await pRMDbContext.ToListAsync());
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                
                .Include(t => t.Owner)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
           return View();
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string username, string password, Tenant model)
        {

            int ownerId = 1;  
            bool isSessionActive = false;

            
            var ownerIdString = HttpContext.Session.GetString("OwnerId");
            if (!string.IsNullOrEmpty(ownerIdString) && int.TryParse(ownerIdString, out int sessionOwnerId))
            {
                ownerId = sessionOwnerId;
                isSessionActive = true;
            }


            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Username is required.");
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Password is required.");
            }

            
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please correct the errors in the form.";
                return View(model);
            }

            try
            {
               
                var newUser = new Users
                {
                    Username = username,
                    Password = password, 
                    Email = model.Email,
                    Role = UserRole.Tenant
                };

                
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                if (newUser.UserId <= 0)
                {
                    ViewBag.ErrorMessage = "Failed to generate UserId for the tenant.";
                    return View(model);
                }

                
                model.UserId = newUser.UserId;
                model.OwnerId = ownerId;

                
                _context.Tenants.Add(model);
                await _context.SaveChangesAsync();

                if (!isSessionActive)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }






        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
           
            ViewData["OwnerId"] = new SelectList(_context.PropertyOwners, "OwnerId", "OwnerId", tenant.OwnerId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tenant.UserId);
            return View(tenant);
        }

        // POST: Tenants/Edit        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TenantId,Name,Email,Phone,UserId,OwnerId,ManagerId")] Tenant tenant)
        {
            if (id != tenant.TenantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.TenantId))
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
            
            ViewData["OwnerId"] = new SelectList(_context.PropertyOwners, "OwnerId", "OwnerId", tenant.OwnerId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tenant.UserId);
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenants
                
                .Include(t => t.Owner)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TenantId == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TenantExists(int id)
        {
            return _context.Tenants.Any(e => e.TenantId == id);
        }


        public async Task<IActionResult> TenantDashboard(int id)
        {
            var manager = await _context.Tenants.FirstOrDefaultAsync(m => m.UserId == id);
            if (manager == null) return NotFound();

            return View(manager);

            //var tenant = _context.Tenants.FirstOrDefault();
            //if (tenant == null)
            //{
            //    return NotFound();
            //}
            //return View(tenant);
        }

        public async Task<IActionResult> ViewAvailableApartments()
        {
            var availableApartments = await _context.Apartments
            .Include(a => a.Building)  
            .Include(a => a.Manager)    
            .Where(a => a.Status == ApartmentStatus.Available)
            .ToListAsync();

            return View(availableApartments);
        }

        public IActionResult MakeAppointment()
        {
            // Obtener lista de managers disponibles para citas
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAppointment([Bind("AppointmentId,Date,Status,ManagerId")] Appointments appointment)
        {
            if (ModelState.IsValid)
            {
                // Asignar el tenant autenticado a la cita
                var tenant = _context.Tenants.FirstOrDefault(); // Obtener el Tenant autenticado
                appointment.TenantId = tenant.TenantId;

                // Guardar la cita
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TenantDashboard)); // Redirigir al Tenant Dashboard
            }

            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Name", appointment.ManagerId);
            return View(appointment);
        }

        public IActionResult SendMessage()
        {
            // Obtener lista de managers disponibles para enviar mensajes
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage([Bind("MessageId,Content,ManagerId")] Message message)
        {
            if (ModelState.IsValid)
            {
                // En lugar de 'SenderTenantId', utiliza la relación con el Tenant ya existente
                var tenant = _context.Tenants.FirstOrDefault(); // Obtener el Tenant autenticado

                // Verifica si el tenant fue encontrado
                if (tenant == null)
                {
                    return NotFound(); // Si no se encuentra el tenant, devuelves un error
                }

                // Asignar el tenant autenticado como remitente
                message.Tenant = tenant;

                // Guardar el mensaje
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TenantDashboard)); // Redirigir al Tenant Dashboard
            }

            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "Name", message.ManagerId);
            return View(message);
        }

    }
}
