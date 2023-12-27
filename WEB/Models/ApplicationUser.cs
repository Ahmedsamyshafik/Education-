using Microsoft.AspNetCore.Identity;

namespace WEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsInRole { get; set; }

        public bool joinCourse { get; set; }

        public string? ExamName { get; set; }

        public int? ExamDegree { get; set; }

        public string? DeviceId { get; set; }

        public List<CoursesNames>? myCourses { get; set; }

        public Alldegrees? degree { get; set; }

    }
    public enum Alldegrees
    {
        الأول = 1,
        الثاني = 2,
        الثالث = 3,
        آخري = 4,
        أدمن = 5,
    }
    public enum Semester
    {
        الأول = 1,
        الثاني = 2,

    }
}
