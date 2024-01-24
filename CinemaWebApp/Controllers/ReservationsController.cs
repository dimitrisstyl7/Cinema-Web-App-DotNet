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
        public async Task<IActionResult> Index(int id)
        {
            var cinemaAppDBContext = _context.Reservations.Include(r => r.Customer).Where(r => id == r.CustomerId).Include(r => r.Screening).Include(r => r.Screening.ScreeningRoom).Include(r => r.Screening.Movie).Include(r => r.Customer.User);
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
        public IActionResult Create(int id)
        {
            int customerId = 2;

            // Check if a reservation already exists for the given customer and screening
            var existingReservation = _context.Reservations
                .FirstOrDefault(r => r.CustomerId == customerId && r.ScreeningId == id);

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
                .FirstOrDefault(s => s.Id == id);

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
        public async Task<IActionResult> Create([Bind("CustomerId,ScreeningId,NoOfBookedSeats")] Reservation reservation)
        {
            ModelState.Remove("Customer");
            ModelState.Remove("Screening");

            // Search for the screening with the specified ScreeningId
            var screening_db = await _context.Screenings.FindAsync(reservation.ScreeningId);

            if (screening_db == null)
            {
                return NotFound();
            }

            int remainingNoOfSeats = screening_db.RemainingNoOfSeats;

            if (reservation.NoOfBookedSeats <= 0)
            {
                ModelState.AddModelError("NoOfBookedSeats", "Please enter a positive number");
            }

            if (reservation.NoOfBookedSeats > remainingNoOfSeats)
            {
                ModelState.AddModelError("NoOfBookedSeats", "Not enough remaining seats");
            }

            if (!ModelState.IsValid)
            {
                var customer = _context.Customers
               .Include(c => c.User) // Include related user information
               .FirstOrDefault(c => c.Id == reservation.CustomerId);

                var screening = _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.ScreeningRoom)
                .FirstOrDefault(s => s.Id == reservation.ScreeningId);

                if (customer == null || screening == null)
                {
                    // Handle case where customer or screening is not found
                    return NotFound();
                }

                // Pass the retrieved data to the view
                ViewData["Customer"] = customer;
                ViewData["Screening"] = screening;

                return View(reservation);
            }

            // Update the remaining number of seats in the screening
            screening_db.RemainingNoOfSeats -= reservation.NoOfBookedSeats;
            _context.Update(screening_db);

            // Add the reservation to the context
            _context.Add(reservation);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the Index action
            return RedirectToAction(nameof(Index), new { id = reservation.CustomerId });
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
