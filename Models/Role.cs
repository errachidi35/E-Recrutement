using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Recrutement.Models
{
	public class Role : IdentityRole<Guid>
	{
		[Display(Name = "Description")]
		[StringLength(256, ErrorMessage = "The description cannot be more than 256 characters.")]
		public string? Description { get; set; }
	}
}
