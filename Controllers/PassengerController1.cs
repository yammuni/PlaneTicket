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
    public class PassengerController : Controller
    {
        private readonly FlightDbContext _context;

        public PassengerController(FlightDbContext context)
        {
            _context = context;
        }

        // GET: Passenger
        public async Task<IActionResult> Index()
        {
            var passengerContext = _context.Passengers.Include(k => k.Reservations);
            return View(await passengerContext.ToListAsync());
        }

        // GET: Passenger/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.PassengerId == id.ToString());
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passenger/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passenger/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Passenger passenger)
        {
            if (passenger != null)
            {
                _context.Passengers.Add(passenger);
                await _context.SaveChangesAsync();
                TempData["msj"] = passenger.FirstName + "  " + passenger.LastName + "You became a member";
                return RedirectToAction(nameof(Index));
            }
            return View(passenger);
        }

        // GET: Passenger/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passenger/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PassengerId,FirstName,LastName,Email,PhoneNumber")] Passenger passenger)
        {
            if (id.ToString() != passenger.PassengerId)
            {
                return NotFound();
            }

            if (passenger != null)
            {
                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassengerExists(int.Parse(passenger.PassengerId)))
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
            return View(passenger);
        }

        // GET: Passenger/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.Include(r => r.Reservations)
                .FirstOrDefaultAsync(m => m.PassengerId == id.ToString());
            if (passenger == null)
            {
                return NotFound();
            }
            if (passenger.Reservations.Count > 0)
            {
                TempData["hata"] = "The Passenger has a reservation first you must Delete his Reservation";
                return NotFound();
            }
            _context.Passengers.Remove(passenger);
            //k.Remove(y);
            _context.SaveChanges();
            TempData["msj"] = passenger.FirstName + "  The Passenger Is deleted";
            return RedirectToAction("Index");

        }

        // POST: Passenger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passengers == null)
            {
                return Problem("Entity set 'FlightDbContext.Passengers'  is null.");
            }
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
            return (_context.Passengers?.Any(e => e.PassengerId == id.ToString())).GetValueOrDefault();
        }
    }
}
