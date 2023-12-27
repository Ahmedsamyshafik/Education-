using WEB.Models;

namespace WEB.IRepo
{
    public interface ICreditCourses
    {
        void Add(CreditCourses course);
        List<CreditCourses> GetAllCourses();
        CreditCourses GetCourse(int Id);
        Task<CreditCourses> GetCoursesAsync(int Id);

		void Save();
        Task<string> SaveAsync();
		Task<string> SaveCourseImge(CreditCourses course);
		void delete(CreditCourses course);

        Task<CreditCourses> GetCourseAsync(int Id);
        Task<string> ChangeNamePriceAsync(int id, string newName, decimal newPrice);


        Task<List<CreditCourses>> GetAllCoursesAsync();

    }
}
