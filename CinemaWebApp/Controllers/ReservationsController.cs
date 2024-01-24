using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public ReservationsController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var cinemaAppDBContext = _context.Reservations.Include(r => r.Customer).Include(r => r.Screening);
            return View(await cinemaAppDBContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)  // Include related user information for the customer
                .Include(r => r.Screening)
                    .ThenInclude(s => s.Movie)  // Include related movie information for the screening
                    .ThenInclude(m => m.Genre) // Include related genre information for the movie
                .Include(r => r.Screening)
                    .ThenInclude(s => s.ScreeningRoom)  // Include related screening room information for the screening
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Set ViewData for Customer and Screening
            ViewData["Customer"] = reservation.Customer;
            ViewData["Screening"] = reservation.Screening;

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id");
            return View();
        }

        // GET: Reservations/BookTickets
        public IActionResult BookTickets(int screeningId, int customerId = 1)
        {
            // Check if a reservation already exists for the given customer and screening
            var existingReservation = _context.Reservations
                .FirstOrDefault(r => r.CustomerId == customerId && r.ScreeningId == screeningId);

            if (existingReservation != null)
            {
                // Reservation already exists, set ViewData to indicate that
                ViewData["ReservationExists"] = true;
                ViewData["ReservationId"] = existingReservation.Id; // Pass the existing reservation ID if needed
                return View();
            }

            // No existing reservation, proceed to retrieve customer, screening, and screening room data
            var customer = _context.Customers
                .Include(c => c.User) // Include related user information
                .FirstOrDefault(c => c.Id == customerId);

            var screening = _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.ScreeningRoom)
                .FirstOrDefault(s => s.Id == screeningId);

            if (customer == null || screening == null)
            {
                // Handle case where customer or screening is not found
                return NotFound();
            }

            // Pass the retrieved data to the view
            ViewData["Customer"] = customer;
            ViewData["Screening"] = screening;

            return View();
        }


        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ScreeningId,NoOfBookedSeats")] Reservation reservation)
        {
            ModelState.Remove("Customer");
            ModelState.Remove("Screening");
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomerId);
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ScreeningId,NoOfBookedSeats")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomerId);
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
