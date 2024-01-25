using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class CinemasController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public CinemasController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Cinemas/Details/{CinemaId}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DateTime currentDate = DateTime.Now;
            DateTime next7Days = currentDate.AddDays(7);

            var cinemaScreenings = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.ScreeningRoom)
                .Where(s => s.ScreeningRoom.CinemaId == id && s.StartTime >= currentDate && s.StartTime < next7Days)
                .OrderBy(s => s.StartTime) // Assuming StartTime is the property representing screening time
                .GroupBy(s => s.Movie)
                .ToListAsync();

            if (cinemaScreenings == null)
            {
                return NotFound();
            }

            if (cinemaScreenings.Count == 0)
            {
                ViewData["message"] = "No screenings in the next 7 days.";
            }

            return View(cinemaScreenings);
        }
    }
}
