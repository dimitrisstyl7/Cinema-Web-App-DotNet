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

        // GET: Sign out user
        public IActionResult SignOut()
        {
            return RedirectToAction(nameof(SignIn));
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

            var db_user = ModelState.IsValid ?
                await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email == user.Email) : null;

            if (db_user == null || !BCrypt.Net.BCrypt.Verify(user.Password, db_user.Password))
            {
                ViewData["Error"] = "Wrong credentials, please try again.";
                return View(user);
            }

            switch (db_user.Role.Name)
            {
                case "customer":
                    int customerId = await _context.Customers
                        .Where(c => c.UserId == db_user.Id)
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                    return RedirectToAction("Index", "Customer", new { id = customerId });
                case "app_admin":
                    int appAdminId = await _context.AppAdmins
                        .Where(aa => aa.UserId == db_user.Id)
                        .Select(aa => aa.Id)
                        .FirstOrDefaultAsync();
                    return RedirectToAction("Index", "AppAdmin", new { id = appAdminId });
                case "content_admin":
                    int contentAdminId = await _context.ContentAdmins
                        .Where(ca => ca.UserId == db_user.Id)
                        .Select(ca => ca.Id)
                        .FirstOrDefaultAsync();
                    return RedirectToAction("Index", "ContentAdmin", new { id = contentAdminId });
                default:
                    ViewData["Error"] = "Something went wrong, please try again.";
                    return View(user);
            }
        }

        // POST: Sign up
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp([Bind("Username, FirstName, LastName, Email, Password")] User user)
        {
            ModelState.Remove(nameof(user.Role));

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool userExist = await _context.Users.AnyAsync(u => u.Email == user.Email || u.Username == user.Username);

            if (userExist)
            {
                ViewData["Error"] = "Email or Username already exists.";
                return View(user);
            }

            user.RoleId = 1; // 1 == User
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 10);
            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SignIn));
        }
    }
}
