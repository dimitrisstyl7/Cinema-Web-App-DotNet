using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class AppAdminsController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public AppAdminsController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: AppAdmins/Index
        public async Task<IActionResult> Index()
        {
            var contentAdmins = await _context.ContentAdmins
                .Include(admin => admin.User)
                .GroupBy(admin => admin.Cinema)
                .ToListAsync();

            if (contentAdmins.Count == 0)
            {
                ViewData["Message"] = "No content admins found.";
            }

            return View(contentAdmins);
        }
    }
}
