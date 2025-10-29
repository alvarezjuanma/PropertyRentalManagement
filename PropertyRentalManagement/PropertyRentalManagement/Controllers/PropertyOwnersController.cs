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
    public class PropertyOwnersController : Controller
    {

        private readonly PRMDbContext _context;

        public PropertyOwnersController(PRMDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var owners = _context.PropertyOwners.ToList();  
            
            if (owners == null || !owners.Any())
            {
                return View("Error");  
            }

            return View(owners); 
        }


        // GET: PropertyOwners
        public async Task<IActionResult> OwnerDashboard(int id)
        {
            //var owner = await _context.PropertyOwners.FirstOrDefaultAsync();
            //return View(owner);

            var owner = await _context.PropertyOwners.FirstOrDefaultAsync(o => o.UserId == id);
            if (owner == null) return NotFound();

            return View(owner);
        }

        /// GET: PropertyOwners/ListEventReports
        public async Task<IActionResult> ListEventReports()
        {
            var eventReports = await _context.EventReports.Include(e => e.Manager).ToListAsync();
            return View(eventReports);  
        }

        // GET: PropertyOwners/Create
        public IActionResult Create()
        {
            return View(); 
        }





        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, PropertyOwner model)
        {
            try
            {
                // Validar el modelo antes de proceder
                if (!ModelState.IsValid)
                {
                    // Retornar la vista con el modelo y los errores
                    return View(model);
                }

                // Crear el nuevo usuario
                var newUser = new Users
                {
                    Username = username,
                    Password = password,
                    Email = model.Email,
                    Role = UserRole.Owner // Asignar el rol de Owner
                };

                // Agregar el nuevo usuario al contexto
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(); // Guardar cambios para generar UserId

                int userId = newUser.UserId;

                // Verificar si el UserId se generó correctamente
                if (userId <= 0)
                {
                    ModelState.AddModelError("UserId", "Failed to generate UserId.");
                    return View(model);
                }

                // Asignar el UserId al modelo PropertyOwner
                model.UserId = userId;

                // Agregar el nuevo propietario al contexto
                _context.PropertyOwners.Add(model);
                await _context.SaveChangesAsync(); // Guardar cambios para PropertyOwner

                // Redirigir a la acción deseada después de la creación
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar cualquier error y mostrar un mensaje
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View(model); // Retornar la vista con el modelo en caso de error
            }

            return RedirectToAction("Index");

        }



    }
}
