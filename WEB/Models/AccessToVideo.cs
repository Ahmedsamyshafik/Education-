namespace WEB.Models
{
    public class AccessToVideo
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Link { get; set; }

        public List<CoursesNames>? myCourses { get; set; }

        public Alldegrees Degree { get; set; } = Alldegrees.الأول;

        public Semester Semester { get; set; } = Semester.الأول;

        public bool Access { get; set; }

        public DateTime Date { get; set; }= DateTime.Now;
    }
}
