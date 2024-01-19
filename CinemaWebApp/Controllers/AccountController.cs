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
            ModelState.Remove(nameof(user.FirstName));
            ModelState.Remove(nameof(user.LastName));
            ModelState.Remove(nameof(user.Username));
            ModelState.Remove(nameof(user.Role));

            var db_user = ModelState.IsValid ? await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email) : null;

            if (db_user == null || !BCrypt.Net.BCrypt.Verify(user.Password, db_user.Password))
            {
                ViewData["Error"] = "Wrong credentials, please try again.";
                return View(user);
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
