using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class FreeCourses
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }

        public bool Delete { get; set; }

		public DateTime Date { get; set; } = DateTime.Now;

	}
}
