using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class QuizTable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<Question>? Questions { get; set; }
        public string? FileNameMaster { get; set; }
        public List<CoursesNames>? myCourses { get; set; }
        public bool Access { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile? File { get; set; }
        public Alldegrees Alldegrees { get; set; } = Alldegrees.الأول;
        public Semester Semester { get; set; } = Semester.الأول;
    }
}
