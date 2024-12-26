using System.ComponentModel.DataAnnotations;

namespace E_Recrutement.Models
{
    public class Skill
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter skill name")]
        [StringLength(50, ErrorMessage = "The skill name cannot be more than 50 characters.")]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        [Display(Name = "Skill logo")]
        [Required(ErrorMessage = "Please enter skill logo")]
        public string Logo { get; set; }
        public Sector? Sector { get; set; }
        [Display(Name = "Sector")]
        public int SectorId { get; set; }
        public bool? Disable { get; set; }
        public virtual ICollection<Offer>? Offers { get; set; }
        public int Popular { get; set; }
    }
}
