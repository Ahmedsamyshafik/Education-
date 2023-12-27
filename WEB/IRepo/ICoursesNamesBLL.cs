using WEB.Models;

namespace WEB.IRepo
{
    public interface ICoursesNamesBLL
    {
        List<CoursesNames> GetAll();
        List<string> GetMyCoursesNames(string userId); // Send U_Id To get his courses
        string GetMyCourseName(int CourseId);
        void save();
        CoursesNames GetMyCourseToAccessPage(int CourseId); // By Course ID
        List<string> GetJoinedCourses(ApplicationUser user, string CourseName);
        void Add(CoursesNames CN);
        CoursesNames GetById(int id); // CourseNames By Id
        List<int> GetVideosByCourseId(int CourseId);
        List<CoursesNames> GetAllForUsers();
        List<CoursesNames> GetAllForQuiz(int courseId);
        List<CoursesNames> ToGetPdfsWithCourses(int Co_id);
        void Delete(CoursesNames id);
        void DeletPdfFromCourses(int pdfId, int Cid);
        void DeletVidFromCourses(int VidId, int Cid);
        void DeletQuizFromCourses(int QuizId, int Cid);



	}
}
