using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        private readonly CinemaAppDBContext _context;

        public AccountController(CinemaAppDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("Email, Password")] User user)
        {
            var db_user = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (db_user == null)
            {
                return NotFound("User not found");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, db_user.Password);

            if (!isPasswordValid)
            {
                return NotFound("Invalid password");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind("Username, Firstname, Lastname, Email, Password")] User user)
        {
            var db_user = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (db_user == null)
            {
                return NotFound("User not found");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, db_user.Password);

            if (!isPasswordValid)
            {
                return NotFound("Invalid password");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
