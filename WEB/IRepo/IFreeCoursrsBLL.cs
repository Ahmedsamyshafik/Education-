using WEB.Models;

namespace WEB.IRepo
{
    public interface IFreeCoursrsBLL
    {
        void SaveCours(FreeCourses course);
        List<FreeCourses> GetAll();
        FreeCourses GetById(int id);
        void remove(FreeCourses freeCourses);
    }
}
