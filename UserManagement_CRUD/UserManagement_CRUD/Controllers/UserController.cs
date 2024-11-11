using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement_CRUD.Models;

namespace UserManagement_CRUD.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // To retrieve the users list
        public IActionResult Index()
        {
            var users = _context.Users.ToList();

            return View(users);
        }

        // To create the new users
        public IActionResult Create()
        {
            return View();
        }

        // To post the user details to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                if (_context.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                    return View(user);
                }

                // Encrypt password using MD5
                using (MD5 md5 = MD5.Create())
                {
                    user.Password = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
                }

                user.CreatedBy = "Admin";
                user.CreatedDate = DateTime.Now;
                user.Active = true;

                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,UserName,MobileNumber,Address,EmailAddress,Password,Active")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        using (var md5 = MD5.Create())
                        {
                            user.Password = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
                        }
                    }

                    user.CreatedDate = DateTime.Now; // Update the created date
                    user.CreatedBy = User.Identity.Name ?? "Admin"; // Assuming the user is logged in

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

