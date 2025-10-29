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
    public class ApartmentsController : Controller
    {
        private readonly PRMDbContext _context;

        public ApartmentsController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var pRMDbContext = _context.Apartments.Include(a => a.Building).Include(a => a.Manager);
            return View(await pRMDbContext.ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartments = await _context.Apartments
                .Include(a => a.Building)
                .Include(a => a.Manager)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);
            if (apartments == null)
            {
                return NotFound();
            }

            return View(apartments);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            var managerIdString = HttpContext.Session.GetString("ManagerId");
            Console.WriteLine($"ManagerId in session (GET): {managerIdString}");

            ViewBag.StatusOptions = new SelectList(Enum.GetValues(typeof(ApartmentStatus)));
            ViewBag.BuildingList = new SelectList(_context.Buildings, "BuildingId", "Name");
            ViewBag.ManagerId = HttpContext.Session.GetString("ManagerId");

            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string apartmentNumber, ApartmentStatus status, Apartments model)
        {
            int managerId = 0;

            var managerIdString = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerIdString) || !int.TryParse(managerIdString, out  managerId))
            {
                ModelState.AddModelError("ManagerId", "Manager is not logged in or ManagerId is invalid.");
                ViewBag.ErrorMessage = "Manager ID is missing or invalid. Please log in as a Manager.";
                Console.WriteLine("Error: ManagerId is missing or invalid. Check session and login.");
            }
            else
            {
                // Si ManagerId es válido, asignarlo al modelo y mostrar el valor en la consola
                model.ManagerId = managerId;
                Console.WriteLine($"ManagerId is valid: {managerId}");
            }

            // Validar BuildingId
            if (model.BuildingId <= 0)
            {
                ModelState.AddModelError("BuildingId", "Building ID is required and must be valid.");
                ViewBag.ErrorMessage = "Building ID is missing or invalid. Please select a valid Building.";
                Console.WriteLine("Error: BuildingId is missing or invalid.");
            }

            if (ModelState.ContainsKey("ManagerId") && ModelState["ManagerId"].Errors.Count > 0)
            {
                Console.WriteLine("ManagerId has the following validation errors:");
                foreach (var error in ModelState["ManagerId"].Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.ContainsKey("BuildingId") && ModelState["BuildingId"].Errors.Count > 0)
            {
                Console.WriteLine("BuildingId has the following validation errors:");
                foreach (var error in ModelState["BuildingId"].Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            var managerExists = _context.PropertyManagers.Any(m => m.ManagerId == managerId);
            var buildingExists = _context.Buildings.Any(b => b.BuildingId == model.BuildingId);

            if (!managerExists)
            {
                ModelState.AddModelError("ManagerId", "The specified Manager does not exist.");
                Console.WriteLine("Error: The specified ManagerId does not exist in the database.");
            }

            if (!buildingExists)
            {
                ModelState.AddModelError("BuildingId", "The specified Building does not exist.");
                Console.WriteLine("Error: The specified BuildingId does not exist in the database.");
            }
                       

            try
            {
                
                model.ApartmentNumber = apartmentNumber;
                model.Status = status;

                
                _context.Apartments.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }


        // GET: Apartments/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            // Cargar opciones de Status
            ViewBag.StatusOptions = new SelectList(
                Enum.GetValues(typeof(ApartmentStatus)).Cast<ApartmentStatus>().Select(e => new { Value = (int)e, Text = e.ToString() }),
                "Value", "Text", apartment.Status
            );

            // Cargar lista de edificios para BuildingId
            ViewBag.BuildingId = new SelectList(_context.Buildings, "BuildingId", "Name", apartment.BuildingId);

            // Cargar lista de managers para ManagerId
            ViewBag.ManagerId = new SelectList(_context.PropertyManagers, "ManagerId", "Name", apartment.ManagerId);

            return View(apartment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyRentalManagement.Models.Apartments model)
        {
            // Verificar que el ID del apartamento coincide
            if (id != model.ApartmentId)
            {
                return NotFound();
            }                

            try
            {                
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(model.ApartmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }


        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartments = await _context.Apartments
                .Include(a => a.Building)
                .Include(a => a.Manager)
                .FirstOrDefaultAsync(m => m.ApartmentId == id);
            if (apartments == null)
            {
                return NotFound();
            }

            return View(apartments);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartments = await _context.Apartments.FindAsync(id);
            if (apartments != null)
            {
                _context.Apartments.Remove(apartments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentsExists(int id)
        {
            return _context.Apartments.Any(e => e.ApartmentId == id);
        }
    }
}
