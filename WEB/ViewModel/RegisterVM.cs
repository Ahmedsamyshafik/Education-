using System.ComponentModel.DataAnnotations;
using WEB.Models;

namespace WEB.ViewModel
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
		public string Phone { get; set; }
		[DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? Password2 { get; set; }
        public Alldegrees Degree { get; set; }
    }
}
