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
    public class BuildingsController : Controller
    {
        private readonly PRMDbContext _context;

        public BuildingsController(PRMDbContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            var pRMDbContext = _context.Buildings.Include(b => b.Manager);
            return View(await pRMDbContext.ToListAsync());
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildings = await _context.Buildings
                .Include(b => b.Manager)
                .FirstOrDefaultAsync(m => m.BuildingId == id);
            if (buildings == null)
            {
                return NotFound();
            }

            return View(buildings);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId");
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Name,Address,ManagerId")] Buildings buildings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId", buildings.ManagerId);
            return View(buildings);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildings = await _context.Buildings.FindAsync(id);
            if (buildings == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId", buildings.ManagerId);
            return View(buildings);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuildingId,Name,Address,ManagerId")] Buildings buildings)
        {
            if (id != buildings.BuildingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingsExists(buildings.BuildingId))
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
            ViewData["ManagerId"] = new SelectList(_context.PropertyManagers, "ManagerId", "ManagerId", buildings.ManagerId);
            return View(buildings);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildings = await _context.Buildings
                .Include(b => b.Manager)
                .FirstOrDefaultAsync(m => m.BuildingId == id);
            if (buildings == null)
            {
                return NotFound();
            }

            return View(buildings);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildings = await _context.Buildings.FindAsync(id);
            if (buildings != null)
            {
                _context.Buildings.Remove(buildings);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingsExists(int id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }
    }
}
