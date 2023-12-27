using WEB.Models;

namespace WEB.IRepo
{
    public interface IAccountBLL
    {
        IEnumerable<ApplicationUser> GetUsers();
		IEnumerable<ApplicationUser> GetSchoolUsers();
		void DeleteCreditCourseStd(int id);
		IEnumerable<ApplicationUser> GetAdmins();


	}
}
