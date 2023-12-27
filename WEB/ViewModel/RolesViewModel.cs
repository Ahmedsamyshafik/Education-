using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModel
{
    public class RolesViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
