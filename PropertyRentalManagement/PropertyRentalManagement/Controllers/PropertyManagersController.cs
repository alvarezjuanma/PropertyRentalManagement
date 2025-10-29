using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PropertyRentalManagement.Data;
using PropertyRentalManagement.Models;

namespace PropertyRentalManagement.Controllers
{
    public class PropertyManagersController : Controller
    {
        private readonly PRMDbContext _context;

        public PropertyManagersController(PRMDbContext context)
        {
            _context = context;
        }


        // GET: PropertyManagers
        public async Task<IActionResult> Index()
        {
            try
            {
                var managers = await _context.PropertyManagers.ToListAsync();
                return View(managers);
            }
            catch (Exception ex)
            {
                // Maneja el error aquí (puedes registrar el error o mostrar un mensaje)
                ViewBag.ErrorMessage = $"Ocurrió un error al cargar los administradores: {ex.Message}";
                return View(new List<PropertyManager>()); // O puedes retornar un mensaje de error
            }
        }


        // GET: PropertyManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: PropertyManagers/ManagerDashboard
        public async Task<IActionResult> ManagerDashboard(int id)
        {
            if (id <= 0) return NotFound();
            var manager = await _context.PropertyManagers.FirstOrDefaultAsync(m => m.UserId == id);
            if (manager == null) return NotFound();

            return View(manager);


            //var manager = await _context.PropertyManagers.FirstOrDefaultAsync();
            //if (manager == null)
            //{
            //    return NotFound();
            //}

            //return View(manager);
        }

        /// GET: PropertyManagers/ListEventReports
        public async Task<IActionResult> ListEventReports()
        {
            var eventReports = await _context.EventReports.Include(e => e.Manager).ToListAsync();
            return View(eventReports);
        }

        //public IActionResult CreateBuilding()
        //{
        //    return RedirectToAction("Create", "Buildings");
        //}

        //public IActionResult CreateTenant()
        //{
        //    return RedirectToAction("Create", "Tenants");
        //}


        // CRUD Operations for Buildings
        public async Task<IActionResult> ManageBuildings()
        {
            var buildings = await _context.Buildings.ToListAsync();
            return View(buildings);
        }

        // CRUD Operations for Apartments
        public async Task<IActionResult> ManageApartments()
        {
            var apartments = await _context.Apartments.ToListAsync();
            return View(apartments);
        }

        // Track apartment status
        public async Task<IActionResult> TrackApartmentStatus()
        {
            var apartments = await _context.Apartments.ToListAsync();
            return View(apartments);
        }

        // Schedule appointments with tenants
        public async Task<IActionResult> ScheduleAppointments()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return View(appointments);
        }

        // Respond to tenants' messages
        public async Task<IActionResult> Messages()
        {
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }

        // GET: PropertyManagers/ReportEvent
        public IActionResult ReportEvent()
        {
            return View();
        }

        // POST: PropertyManagers/ReportEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportEvent(EventReport eventReport)
        {
            if (ModelState.IsValid)
            {
                eventReport.ManagerId = 1;
                eventReport.ReportedDate = DateTime.Now;
                _context.Add(eventReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManagerDashboard), new { id = eventReport.ManagerId });
            }
            return View(eventReport);
        }

        // Este método lista los reportes para ser vistos por el Property Owner
        public async Task<IActionResult> EventReports()
        {
            var eventReports = await _context.EventReports.Include(e => e.Manager).ToListAsync();
            return View(eventReports);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string username, string password, PropertyManager model)
        {
            var ownerIdString = HttpContext.Session.GetString("OwnerId");

            // Verifica que OwnerId esté presente en la sesión
            if (string.IsNullOrEmpty(ownerIdString) || !int.TryParse(ownerIdString, out int ownerId))
            {
                ViewBag.ErrorMessage = "Owner is not logged in.";
                return View();
            }

            try
            {
                // Crear nuevo usuario
                var newUser = new Users
                {
                    Username = username,
                    Password = password,
                    Email = model.Email,
                    Role = UserRole.Manager
                };

                // Agregar el usuario al contexto
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(); // Esto genera el UserId

                // Verificar si se generó el UserId
                if (newUser.UserId <= 0)
                {
                    ViewBag.ErrorMessage = "Failed to generate UserId.";
                    return View(model);
                }

                // Asignar UserId y OwnerId al modelo
                model.UserId = newUser.UserId;
                model.OwnerId = ownerId;

                // Agregar el PropertyManager al contexto y guardar
                _context.PropertyManagers.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var propertyManager = await _context.PropertyManagers.FindAsync(id);
            if (propertyManager == null)
            {
                return NotFound();
            }
            return View(propertyManager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyManager model)
        {
            // Verifica que el ID en la URL coincida con el ManagerId del modelo
            if (id != model.ManagerId)
            {
                ViewBag.ErrorMessage = "The ID in the URL does not match the model ID.";
                return View(model);
            }

            // Verifica que el estado del modelo sea válido antes de proceder
            if (!ModelState.IsValid)
            {
                var errorList = new List<string>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errorList.Add($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                ViewBag.ErrorList = errorList;

                ViewBag.ErrorMessage = "There are errors in the form. Please check the fields and try again.";
                return View(model);
            }

            try
            {
                // Carga el registro existente para obtener los IDs correctos
                var existingManager = await _context.PropertyManagers.AsNoTracking().FirstOrDefaultAsync(pm => pm.ManagerId == id);
                if (existingManager == null)
                {
                    ViewBag.ErrorMessage = "No property manager found with the specified ID.";
                    return View(model);
                }

                // Asigna los valores de UserId y OwnerId desde el registro existente
                model.UserId = existingManager.UserId;
                model.OwnerId = existingManager.OwnerId;

                // Actualiza el registro con los valores que pueden ser modificados
                _context.PropertyManagers.Update(model);
                await _context.SaveChangesAsync();

                // Redirige a la página de índice después de guardar los cambios
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ViewBag.ErrorMessage = "Concurrency error occurred while updating the record.";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View(model);
            }
        }

        // GET: PropertyManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyManager = await _context.PropertyManagers
                .FirstOrDefaultAsync(pm => pm.ManagerId == id);
            if (propertyManager == null)
            {
                return NotFound();
            }

            return View(propertyManager);
        }


        // POST: PropertyManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                // Cargar el PropertyManager junto con el User asociado
                var propertyManager = await _context.PropertyManagers
                    .Include(pm => pm.User)
                    .FirstOrDefaultAsync(pm => pm.ManagerId == id);

                if (propertyManager == null)
                {
                    ViewBag.ErrorMessage = "Property Manager not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Elimina el User asociado si existe
                if (propertyManager.UserId > 0)
                {
                    var user = await _context.Users.FindAsync(propertyManager.UserId);
                    if (user != null)
                    {
                        _context.Users.Remove(user);
                    }
                }

                // Elimina el PropertyManager
                _context.PropertyManagers.Remove(propertyManager);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while trying to delete the Property Manager: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }


        }














    }
}
