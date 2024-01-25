using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;
using System.Text.RegularExpressions;

namespace CinemaWebApp.Controllers
{
    public partial class MoviesController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public MoviesController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Movies/Details/{MovieId}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreId,Title,Duration,Content,Description,ReleaseYear,Director")] Movie movie)
        {
            ModelState.Remove(nameof(movie.Genre));
            string releaseYear = movie.ReleaseYear;

            if (!ReleaseYearRegex().IsMatch(releaseYear))
            {
                ModelState.AddModelError("ReleaseYear", "Please enter a valid year.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
                return View(movie);
            }

            bool movieExists = _context.Movies.Any(m => m.Title == movie.Title && m.ReleaseYear == movie.ReleaseYear);

            if (movieExists)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
                ViewData["Error"] = "Movie already exists.";
                return View(movie);
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ContentAdmins");
        }

        // GET: Movies/Edit/{MovieId}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        // POST: Movies/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,GenreId,Title,Duration,Content,Description,ReleaseYear,Director")] Movie movie)
        {
            ModelState.Remove(nameof(movie.Genre));

            if (movie.Id == 0)
            {
                return NotFound();
            }

            string releaseYear = movie.ReleaseYear;

            if (!ReleaseYearRegex().IsMatch(releaseYear))
            {
                ModelState.AddModelError("ReleaseYear", "Please enter a valid year.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
                return View(movie);
            }

            bool movieExists = _context.Movies.Any(m => m.Title == movie.Title && m.ReleaseYear == movie.ReleaseYear && m.Id != movie.Id);

            if (movieExists)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
                ViewData["Error"] = "Movie already exists.";
                return View(movie);
            }

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "ContentAdmins");
        }

        // GET: Movies/Delete/{MovieId}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/{MovieId}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.Where(m => m.Id == id).Include(m => m.Screenings).ThenInclude(s => s.Reservations).FirstAsync();
            if (movie != null)
            {
                _context.Reservations.RemoveRange(movie.Screenings.SelectMany(s => s.Reservations));
                _context.Screenings.RemoveRange(movie.Screenings);
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ContentAdmins");
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        [GeneratedRegex(@"(?:19|20)[0-9]{2}")]
        private static partial Regex ReleaseYearRegex();
    }
}
