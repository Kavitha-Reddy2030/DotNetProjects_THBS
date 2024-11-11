using System.ComponentModel.DataAnnotations;

namespace PackageAPI_UOW.Models.Domain
{
    public class State
    {
        [Key]
        public int StateId { get; set; } // Primary Key
        public string StateName { get; set; }


        public int CountryId { get; set; } // Foreign Key
        public Country Country { get; set; } // Navigation property

         public ICollection<City> Cities { get; set; }
    }
}
