using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementApplication.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Name is Required.")]
        [StringLength(100)]
        //[Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^\+?[0-9]{1,14}$", ErrorMessage = "Invalid Mobile Number. Please enter a valid numeric value.")]
        public string MobileNumber { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email Address is Required.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }

    public class UserContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
    }
}
