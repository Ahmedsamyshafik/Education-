using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
    public class FreeCoursrsBLL : IFreeCoursrsBLL
    {
        private readonly ApplicationContext db;

        public FreeCoursrsBLL(ApplicationContext db)
        {
            this.db = db;
        }

        public List<FreeCourses> GetAll()
        {
            return db.freeCourses.ToList();
        }
        public FreeCourses GetById(int id) {
            return db.freeCourses.FirstOrDefault(x => x.Id == id);
        }
  
        public void SaveCours(FreeCourses course)
        {
            // Save IMG !
            db.freeCourses.Add(course);
            db.SaveChanges();
        }

        void IFreeCoursrsBLL.remove(FreeCourses freeCourses)
        {
          db.Remove(freeCourses);
            db.SaveChanges();
        }
    }
}
