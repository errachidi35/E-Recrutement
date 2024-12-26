using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace E_Recrutement.ViewModel
{
    public class UserRolesViewModel
    {
        public Guid RoleId { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
