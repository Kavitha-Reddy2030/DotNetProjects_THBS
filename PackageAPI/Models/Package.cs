// using System.ComponentModel.DataAnnotations;
// using System.Text.Json.Serialization;

// namespace PackageAPI.Models
// {
//     public class Package
//     {
//         [Key]
//         public int PackageId { get; set; } // Primary Key

//         [Required]
//         public string PackageName { get; set; }

//         [Required]
//         public string Description { get; set; }

//         [Required]
//         public decimal Price { get; set; }

//        //[JsonIgnore]
//         public virtual ICollection<PackageCity> PackageCities { get; set; }
//         public virtual ICollection<PackageResort> PackageResorts { get; set; }
//     }
// }

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace PackageAPI.Models
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; } // Primary Key

        [Required]
        public string PackageName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now; // Auto-set to current date/time on creation

        public bool Status { get; set; } = true; // Default value is true when package is created

        public virtual ICollection<PackageCity> PackageCities { get; set; }
        public virtual ICollection<PackageResort> PackageResorts { get; set; }
    }
}

