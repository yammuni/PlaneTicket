using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flight_Reservation_System.Data;
using Flight_Reservation_System.Models;

namespace Flight_Reservation_System.Controllers
{
    public class AirPlaneController : Controller
    {
        private readonly FlightDbContext _context;

        public AirPlaneController(FlightDbContext context)
        {
            _context = context;
        }

        // GET: AirPlane
        public async Task<IActionResult> Index()
        {
            return _context.AirPlanes != null ?
                        View(await _context.AirPlanes.ToListAsync()) :
                        Problem("Entity set 'FlightDbContext.AirPlanes'  is null.");
        }

        // GET: AirPlane/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AirPlanes == null)
            {
                return NotFound();
            }

            var airPlane = await _context.AirPlanes
                .FirstOrDefaultAsync(m => m.PlaneId == id);
            if (airPlane == null)
            {
                return NotFound();
            }

            return View(airPlane);
        }

        // GET: AirPlane/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirPlane/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaneId,PlaneName,Capacity")] AirPlane airPlane)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airPlane);
                await _context.SaveChangesAsync();
                TempData["msj"] = airPlane.PlaneName + "Is Created";
                return RedirectToAction(nameof(Index));
            }
            return View(airPlane);
        }

        // GET: AirPlane/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AirPlanes == null)
            {
                return NotFound();
            }

            var airPlane = await _context.AirPlanes.FindAsync(id);
            if (airPlane == null)
            {
                return NotFound();
            }
            return View(airPlane);
        }

        // POST: AirPlane/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaneId,PlaneName,Capacity")] AirPlane airPlane)
        {
            if (id != airPlane.PlaneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airPlane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPlaneExists(airPlane.PlaneId))
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
            return View(airPlane);
        }
        // GET: AirPlane/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AirPlanes == null)
            {
                TempData["error"] = "Please Choose AirPlane TO Delete";
                return NotFound();
            }
            var airPlane = await _context.AirPlanes
                .Include(x => x.Flights)
                .FirstOrDefaultAsync(m => m.PlaneId == id);


            if (airPlane is null)
            {
                TempData["error"] = "Secilen yazar bulunamadi";
                return View("PlaneError");
            }
            if (airPlane.Flights.Count > 0)
            {
                TempData["error"] = "You Want Plane To Delete Flight Will Be Fly. First You Must Delete The Air Plane";
                return View("PlaneError");
            }
            _context.AirPlanes.Remove(airPlane);
            //k.Remove(y);
            _context.SaveChanges();
            TempData["msj"] = airPlane.PlaneName + "Have Been Deleted";
            return RedirectToAction("Index");

        }

        // POST: AirPlane/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AirPlanes == null)
            {
                return Problem("Entity set 'FlightDbContext.AirPlanes'  is null.");
            }
            var airPlane = await _context.AirPlanes.FindAsync(id);
            if (airPlane != null)
            {
                _context.AirPlanes.Remove(airPlane);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirPlaneExists(int id)
        {
            return (_context.AirPlanes?.Any(e => e.PlaneId == id)).GetValueOrDefault();
        }
    }
}

