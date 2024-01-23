using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public ScreeningsController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Screenings
        public async Task<IActionResult> Index()
        {
            var cinemaAppDBContext = _context.Screenings.Include(s => s.Movie).Include(s => s.ScreeningRoom);
            return View(await cinemaAppDBContext.ToListAsync());
        }

        // GET: Screenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.ScreeningRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // GET: Screenings/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["ScreeningRoomId"] = new SelectList(_context.ScreeningRooms, "Id", "Name");
            return View();
        }

        // POST: Screenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,ScreeningRoomId,StartTime")] Screening screening)
        {
            ModelState.Remove(nameof(screening.Movie));
            ModelState.Remove(nameof(screening.ScreeningRoom));
            
            if (ModelState.IsValid)
            {
                var remNumOfseats = _context.ScreeningRooms.Where(s => s.Id == screening.ScreeningRoomId).Select(s => s.TotalNoOfSeats).FirstOrDefault();
                screening.RemainingNoOfSeats = remNumOfseats;
                _context.Add(screening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", screening.MovieId);
            ViewData["ScreeningRoomId"] = new SelectList(_context.ScreeningRooms, "Id", "Name", screening.ScreeningRoomId);
            return View(screening);
        }

        // GET: Screenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", screening.MovieId);
            ViewData["ScreeningRoomId"] = new SelectList(_context.ScreeningRooms, "Id", "Name", screening.ScreeningRoomId);
            return View(screening);
        }

        // POST: Screenings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,ScreeningRoomId,StartTime,RemainingNoOfSeats")] Screening screening)
        {
            ModelState.Remove(nameof(screening.Movie));
            ModelState.Remove(nameof(screening.ScreeningRoom));

            if (id != screening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var remNumOfseats = _context.ScreeningRooms.Where(s => s.Id == screening.ScreeningRoomId).Select(s => s.TotalNoOfSeats).FirstOrDefault();
                    screening.RemainingNoOfSeats = remNumOfseats;
                    _context.Update(screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.Id))
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

            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", screening.MovieId);
            ViewData["ScreeningRoomId"] = new SelectList(_context.ScreeningRooms, "Id", "Name", screening.ScreeningRoomId);
            return View(screening);
        }

        // GET: Screenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.ScreeningRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening != null)
            {
                _context.Screenings.Remove(screening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }
    }
}
