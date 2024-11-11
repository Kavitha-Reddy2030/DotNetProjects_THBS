using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApplication.Models;

namespace StudentManagementApplication.Controllers
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
    }
}
