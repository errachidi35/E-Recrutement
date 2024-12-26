using System.ComponentModel.DataAnnotations;

namespace E_Recrutement.Models
{
	public class Profil
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter profil name")]
		[StringLength(50, ErrorMessage = "The profil name cannot be more than 50 characters.")]
		public string Name { get; set; }

        public Sector? Sector { get; set; }
        [Display(Name = "Sector")]
        public int SectorId { get; set; }
        public bool? Disable { get; set; }
        [Required]
        public string Slug { get; set; }

        public virtual ICollection<Offer>? Offers { get; set; }
        public int Popular { get; set; }

    }
}
