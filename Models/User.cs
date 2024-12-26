
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

using System.Diagnostics.Metrics;



namespace E_Recrutement.Models
{
	public class User : IdentityUser<Guid>
	{
		//User
		[Display(Name = "Full Name")]
		[StringLength(100, ErrorMessage = "Full name cannot be more than 100 characters.")]
		public string? FullName { get; set; }

		[Display(Name = "Age")]
		[Range(0, 100, ErrorMessage = "Please enter valid age.")]
		public int? Age { get; set; }

		[Display(Name = "Phone")]
		[StringLength(20, ErrorMessage = "Please enter valid phonenumber.", MinimumLength = 9)]
		public string? Phone { get; set; }


		[Display(Name = "Address")]
		public string? Address { get; set; }

		[Display(Name = "Title")]
		[Required(ErrorMessage = "Please enter title name")]
		[StringLength(100, ErrorMessage = "The title name cannot be more than 100 characters.")]
		public string? Title { get; set; }

		[Display(Name = "NbreExpYears")]
		public int? NbreExpYears { get; set; }

		//Recruiter
		[Display(Name = "Contact")]
		public string? Contact { get; set; }

		[Display(Name = "Create date")]
		public DateTime? CreateDate { get; set; }

		[Display(Name = "Logo")]
		public string? UrlAvatar { get; set; }
		
		[Display(Name = "Company overview")]
		public string? Description { get; set; }

		[Display(Name = "Website")]
		[StringLength(50, ErrorMessage = "The website cannot be more than 50 characters.")]
		public string? WebsiteURL { get; set; }

		[Display(Name = "Location")]
		public string? Location { get; set; }
		public int? Status { set; get; } // 0 = denied, 1 = waiting, 2 = confirmed, -1 = admin, null = default

        public string Slug { get; set; }

        public Province? Province { get; set; }
		[Display(Name = "Province")]
		public int? ProvinceId { get; set; }

		[Display(Name = "Company size")]
		public string? CompanySize { get; set; }
		[Display(Name = "Working days")]
		public string? WorkingDays { get; set; }
	
	}
}
