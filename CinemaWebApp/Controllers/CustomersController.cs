using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public CustomersController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var cinemaAppDBContext = _context.Cinemas;
            return View(await cinemaAppDBContext.ToListAsync());
        }
    }
}
