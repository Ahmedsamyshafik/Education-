using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class CreditCourses
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }

        public string? imgPath { get; set; }

        [NotMapped]
        public IFormFile? img { get; set; }

        public decimal Price { get; set; }

        public bool join { get; set; }

		public DateTime Date { get; set; } = DateTime.Now;

	}
}
