using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.Data
{
    public class UserRoleDbContext : DbContext
    {
        public UserRoleDbContext(DbContextOptions<UserRoleDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .ToTable("mst_Users");

            // Unique constraint on Email in User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .ToTable("mst_Roles");

            // Seeding initial data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Super Admin", ActiveStatus = true, CreatedOn = DateTime.UtcNow, CreatedBy = "Super Admin" }
            );

            // Seeding initial data for Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "Super Admin",
                    Email = "superadmin@example.com",
                    Password = HashPassword("SuperAdmin@123"),
                    MobileNumber = "9876543210",
                    RoleId = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Super Admin",
                    ActiveStatus = true
                }
            );
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword;
            }
        }
    }
}
