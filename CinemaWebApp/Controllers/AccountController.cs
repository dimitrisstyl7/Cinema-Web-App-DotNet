using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public AccountController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: Sign in page
        public IActionResult SignIn()
        {
            return View();
        }

        // GET: Sign up page
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: Sign in
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn([Bind("Email, Password")] User user)
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

            return RedirectToAction("Details", "Users", new { id = db_user.Id });
        }

        // POST: Sign up
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp([Bind("Username, FirstName, LastName, Email, Password")] User user)
        {
            user.RoleId = 1;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 10);
            _context.Add(user);
            await _context.SaveChangesAsync();
            return View(nameof(SignIn));
        }
    }
}
