using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace E_Recrutement.Models
{
    public class Province
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter province name")]
        [StringLength(50, ErrorMessage = "The province name cannot be more than 50 characters.")]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public Sector? Sector { get; set; }
        [Display(Name = "Sector")]
        public int SectorId { get; set; }
        public bool? Disable { get; set; }

        public ICollection<Offer>? Offers { get; set; }
        public int Popular { get; set; }
    }
}