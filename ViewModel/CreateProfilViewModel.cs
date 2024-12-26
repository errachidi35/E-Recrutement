using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace E_Recrutement.ViewModel
{
    public class CreateProfilViewModel
    {
        [Required(ErrorMessage = "Please enter profil name")]
        [StringLength(50, ErrorMessage = "The profil name cannot be more than 50 characters.")]
        public string Name { get; set; }
  
    }
}
