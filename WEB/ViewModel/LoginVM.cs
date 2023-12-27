using System.ComponentModel.DataAnnotations;
using WEB.Models;

namespace WEB.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public Alldegrees Degree { get; set; }
    }
}
