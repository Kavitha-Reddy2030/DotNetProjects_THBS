using Microsoft.EntityFrameworkCore;
using PackageAPI_UOW.Models.Domain;

namespace PackageAPI_UOW.Data
{
    public class PackageUOWDbContext : DbContext
    {
        public PackageUOWDbContext(DbContextOptions<PackageUOWDbContext> options) : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Resort> Resorts { get; set; }
        public DbSet<PackageCity> PackageCities { get; set; }
        public DbSet<PackageResort> PackageResorts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define Country entity
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.CountryId); // Primary Key
                entity.HasIndex(c => c.CountryName).IsUnique(); // Unique constraint on CountryName
                entity.Property(c => c.CountryName).IsRequired();
            });

            // Define State entity with foreign key to Country
            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(s => s.StateId); // Primary Key
                entity.HasIndex(s => new { s.StateName }).IsUnique(); // Composite unique index on StateName and CountryId
                entity.Property(s => s.StateName).IsRequired();

                // Foreign key to Country
                entity.HasOne(s => s.Country)
                    .WithMany(c => c.States)
                    .HasForeignKey(s => s.CountryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Define City entity with foreign key to State
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(c => c.CityId); // Primary Key
                entity.HasIndex(c => new { c.CityName}).IsUnique(); // Unique constraint on CityName and StateId
                entity.Property(c => c.CityName).IsRequired();

                // Foreign key to State
                entity.HasOne(c => c.State)
                    .WithMany(s => s.Cities)
                    .HasForeignKey(c => c.StateId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Define Resort entity with foreign key to City
            modelBuilder.Entity<Resort>(entity =>
            {
                entity.HasKey(r => r.ResortId); // Primary Key
                entity.Property(r => r.ResortName).IsRequired();

                // Foreign key to City
                entity.HasOne(r => r.City)
                    .WithMany(c => c.Resorts)
                    .HasForeignKey(r => r.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Define Package entity
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(p => p.PackageId); // Primary Key
                entity.Property(p => p.PackageName).IsRequired();
                entity.Property(p => p.Description).IsRequired();
                entity.Property(p => p.Price).IsRequired(); // Assuming Price is a property in the Package class
            });

            // Define PackageCity entity with foreign key to Package and City
            modelBuilder.Entity<PackageCity>(entity =>
            {
                entity.HasKey(pc => pc.PackageCityId); // Primary Key
                entity.Property(pc => pc.Description).IsRequired();

                // Foreign key to Package
                entity.HasOne(pc => pc.Package)
                    .WithMany(p => p.PackageCities)
                    .HasForeignKey(pc => pc.PackageId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to City
                entity.HasOne(pc => pc.City)
                    .WithMany(c => c.PackageCities)
                    .HasForeignKey(pc => pc.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Define PackageResort entity with foreign key to Package and Resort
            modelBuilder.Entity<PackageResort>(entity =>
            {
                entity.HasKey(pr => pr.PackageResortId); // Primary Key
                entity.Property(pr => pr.Description).IsRequired();

                // Foreign key to Package
                entity.HasOne(pr => pr.Package)
                    .WithMany(p => p.PackageResorts)
                    .HasForeignKey(pr => pr.PackageId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to Resort
                entity.HasOne(pr => pr.Resort)
                    .WithMany(r => r.PackageResorts)
                    .HasForeignKey(pr => pr.ResortId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data for Country
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, CountryName = "USA" },
                new Country { CountryId = 2, CountryName = "Canada" }
            );

            // Seed data for State
            modelBuilder.Entity<State>().HasData(
                new State { StateId = 1, StateName = "California", CountryId = 1 },
                new State { StateId = 2, StateName = "Ontario", CountryId = 2 }
            );

            // Seed data for City
            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, CityName = "Los Angeles", StateId = 1 },
                new City { CityId = 2, CityName = "Toronto", StateId = 2 },
                new City { CityId = 3, CityName = "San Francisco", StateId = 1 }
            );

            // Seed data for Package
            modelBuilder.Entity<Package>().HasData(
                new Package { PackageId = 1, PackageName = "Beach Holiday", Description = "A relaxing beach holiday.", Price = 499.99m },
                new Package { PackageId = 2, PackageName = "City Tour", Description = "Explore the city's culture.", Price = 299.99m }
            );

            // Seed data for Resort
            modelBuilder.Entity<Resort>().HasData(
                new Resort { ResortId = 1, ResortName = "Luxury Beach Resort", CityId = 1 },
                new Resort { ResortId = 2, ResortName = "Affordable Beach Resort", CityId = 1 },
                new Resort { ResortId = 3, ResortName = "City Resort", CityId = 2 }
            );

            // Seed data for PackageCity with Description
            modelBuilder.Entity<PackageCity>().HasData(
                new PackageCity {PackageCityId = 1, PackageId = 1, CityId = 1, Description = "Beach Holiday in Los Angeles" },
                new PackageCity { PackageCityId = 2, PackageId = 2, CityId = 2, Description = "City Tour in Toronto" },
                new PackageCity { PackageCityId = 3, PackageId = 1, CityId = 3, Description = "Beach Holiday in San Francisco" }
            );

            // Seed data for PackageResort
            modelBuilder.Entity<PackageResort>().HasData(
                new PackageResort { PackageResortId = 1, PackageId = 1, ResortId = 1, Description = "Luxury Beach Resort" },
                new PackageResort { PackageResortId = 2, PackageId = 1, ResortId = 2, Description = "Affordable Beach Resort" },
                new PackageResort { PackageResortId = 3, PackageId = 2, ResortId = 3, Description = "City Resort with Amenities" }
            );
        }
    }
}
