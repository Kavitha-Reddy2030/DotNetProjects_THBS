using System.ComponentModel.DataAnnotations;

namespace PackageAPI_UOW.Models.Domain
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; } // Primary Key        
        [Required]
        public string CountryName { get; set; }

         public ICollection<State> States { get; set; }
    }
}
