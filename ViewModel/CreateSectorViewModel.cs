using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace E_Recrutement.ViewModel
{
    public class CreateSectorViewModel
    {
        [Required(ErrorMessage = "Please enter sector name")]
        [StringLength(100, ErrorMessage = "Sector name cannot be more than 100 characters.")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "The description cannot be more than 256 characters.")]
        public string? Description { get; set; }
    }
}
