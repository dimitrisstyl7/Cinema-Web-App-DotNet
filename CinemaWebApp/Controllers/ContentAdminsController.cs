using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class ContentAdminsController : Controller
    {
        private readonly CinemaAppDBContext _context;

        public ContentAdminsController(CinemaAppDBContext context)
        {
            _context = context;
        }

        // GET: ContentAdmins/Create/{AppAdminId}
        public IActionResult Create(int id)
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["AppAdminId"] = id;
            return View();
        }

        // POST: ContentAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("User, CinemaId")] ContentAdmin contentAdmin)
        {
            ModelState.Remove(nameof(ContentAdmin.Cinema));
            ModelState.Remove(nameof(ContentAdmin.User) + "." + nameof(ContentAdmin.User.Role));

            if (!ModelState.IsValid)
            {
                ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
                return View(contentAdmin);
            }

            bool userExists = _context.Users.Any(u => u.Email == contentAdmin.User.Email || u.Username == contentAdmin.User.Username);

            if (userExists)
            {
                ViewData["Error"] = "Username or email already exists.";
                ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
                return View(contentAdmin);
            }

            contentAdmin.User.RoleId = 3;
            contentAdmin.User.Password = BCrypt.Net.BCrypt.HashPassword(contentAdmin.User.Password, workFactor: 10);
            _context.Add(contentAdmin);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "AppAdmins", new { id = ViewData["AppAdminId"] });
        }

        // GET: ContentAdmins/Delete/{ContentAdminId}
        public async Task<IActionResult> Delete(int id)
        {
            var contentAdmin = await _context.ContentAdmins.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);

            if (contentAdmin != null)
            {
                contentAdmin.User.ContentAdmins.Remove(contentAdmin);
                _context.Users.Remove(contentAdmin.User);
                _context.ContentAdmins.Remove(contentAdmin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "AppAdmins");
        }
    }
}
