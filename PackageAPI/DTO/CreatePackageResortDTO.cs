using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class CreatePackageResortDTO
    {
        [Required]
        public int ResortId { get; set; }
        
        [Required]
        public string Description { get; set; }

        // You can also include additional fields as needed
        [Required]
        public string ResortName { get; set; }
    }
}