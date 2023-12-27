using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class UsersQuiz
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

		[ForeignKey("QuizTable")]
		public int ExamId { get; set; }

        public string? UserName { get; set; }

        public string? ExamName { get; set; }

        public int ExamDegree { get; set; }

        public int MaxDegree { get; set; }

        public string? StudentFile { get; set; }

        public bool Makalie { get; set; }

        public int CourseId { get; set; } // Credit or CN ~/ ملهاش لازمه اصلا

        public Semester semester { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public QuizTable QuizTable { get; set; }
    }
}
