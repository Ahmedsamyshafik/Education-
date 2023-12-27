using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class CoursesNames
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [ForeignKey("CreditCourses")]
        public int CourseId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        public bool Access { get; set; }

		[ForeignKey("AccessToVideo")]
		public int VidId { get; set; }

		[ForeignKey("QuizTable")]
		public int QuizId { get; set; }

		[ForeignKey("PdfMaterial")]
		public int PdfId { get; set; }

		public DateTime Date { get; set; } = DateTime.Now;

        public ApplicationUser ApplicationUser { get; set; }
        public AccessToVideo AccessToVideo { get; set; }
        public QuizTable QuizTable { get; set; }
        public PdfMaterial PdfMaterial { get; set; }
        public CreditCourses CreditCourses { get; set; }

	}
}
