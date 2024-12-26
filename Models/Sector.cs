using System.ComponentModel.DataAnnotations;

namespace E_Recrutement.Models
{
	public class Sector
	{
		public int Id { get; set; }
		[Display(Name = "Name")]
		[Required(ErrorMessage = "Please enter sector name")]
		[StringLength(100, ErrorMessage = "Sector name cannot be more than 100 characters.")]
		public string Name { get; set; }
		[Display(Name = "Description")]
		[StringLength(256, ErrorMessage = "The description cannot be more than 256 characters.")]
		public string? Description { get; set; }

        [Required]
        public string Slug { get; set; }
        public bool? Disable { get; set; }
        public ICollection<Skill>? Skills { get; set; }
		public ICollection<Profil>? Profils { get; set; }
		public ICollection<Province>? Provinces { get; set; }
		public ICollection<User>? Users { get; set; }

	}
}
