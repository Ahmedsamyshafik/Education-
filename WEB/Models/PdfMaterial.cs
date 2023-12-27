using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class PdfMaterial
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CoursesNames> coursesNames { get; set; }

        public bool Access { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public string? Pdf_Path { get; set; }

        public Semester Semester { get; set; } = Semester.الأول;

        public string? Description { get; set; }

        public Alldegrees alldegrees { get; set; } = Alldegrees.الأول;

		public DateTime Date { get; set; } = DateTime.Now;

	}
}
