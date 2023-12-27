using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
    public class CoursesNamesBLL :ICoursesNamesBLL
    {
        private readonly ApplicationContext db;

        public CoursesNamesBLL(ApplicationContext db)
        {
            this.db = db;
        }

        public CoursesNames GetById(int id)
        {
            var x = db.coursesNames.FirstOrDefault(c => c.Id == id);

			return db.coursesNames.FirstOrDefault(c => c.Id == id);
        }

        public void Delete(CoursesNames  x)
        {
            
                db.coursesNames.Remove(x);
            db.SaveChanges();
            
        }
		public void DeletVidFromCourses(int VidId, int Cid)
		{
			var all = GetAll();
			foreach (var x in all)
			{
				if (x.VidId == VidId && x.CourseId == Cid)
				{
					var vid = GetById(x.Id);
					Delete(vid);
				}
			}
		}
		public void DeletPdfFromCourses(int pdfId,int Cid)
        {
            var all = GetAll();
            foreach (var x in all)
            {
                if(x.PdfId == pdfId && x.CourseId == Cid)
                {
                    var pdf=GetById(x.Id);
                    Delete(pdf);          
                }
            }
        }

        public List<CoursesNames> GetAllForUsers()
        {
            List<CoursesNames> modl = new();
            var all = db.coursesNames.ToList();
            foreach (var item in all)
            {
                if (item.UserId != null)
                {
                    modl.Add(item);
                }
            }
            return modl;
        }

        public List<CoursesNames> GetAllForQuiz(int courseId)
        {
            List<CoursesNames> modl = new();
            var all = db.coursesNames.ToList();
            foreach (var item in all)
            {
                if (item.QuizId > 0 && item.CourseId == courseId)
                {
                    modl.Add(item);
                }
            }
            return modl.OrderByDescending(x=>x.Date).ToList();
        }
        public void DeletQuizFromCourses(int QuizId, int Cid)
        {
            var all = GetAll();
            foreach (var x in all)
            {
                if (x.QuizId == QuizId && x.CourseId == Cid)
                {
                    var quiz = GetById(x.Id);
                    Delete(quiz);
                }
            }
        }

        public List<CoursesNames> GetAll()
        {
            return db.coursesNames.ToList();
        }

        public List<string> GetMyCoursesNames(string userId)
        {
            List<string> mycourses = new List<string>();
            var all = GetAll();
            foreach (var coursesName in all)
            {
                if (coursesName.UserId == userId) mycourses.Add(coursesName.Name);
            }
            return mycourses;
        }

        public List<string> GetJoinedCourses(ApplicationUser user, string CourseName)
        {
            var all = GetAll();
            var JoinedCourses = new List<string>();
            foreach (var coursesName in all)
            {
                //var cid= GetMyCourseName()
                if (coursesName.UserId == user.Id && coursesName.Name == CourseName)
                {
                    JoinedCourses.Add(coursesName.Name);
                }
            }
            return JoinedCourses;
        }

        public string GetMyCourseName(int CourseId)
        {
            //var courses = GetAll();
            var c = db.CeditCourses.FirstOrDefault(x => x.id == CourseId);
            return c.name.ToString();
        }

        public CoursesNames GetMyCourseToAccessPage(int CourseId) // GetByCourseId?
        {
            return db.coursesNames.FirstOrDefault(x => x.CourseId == CourseId);
        }

        public void save()
        {
            db.SaveChanges();
        }

        public void Add(CoursesNames CN)
        {
            CoursesNames SaveMe = new CoursesNames()
            { 
                Access = CN.Access,
                Name = CN.Name,
                CourseId = CN.CourseId,
                PdfId = CN.PdfId,
                QuizId = CN.QuizId,
                UserId = CN.UserId,
                VidId = CN.VidId,
            };
            db.coursesNames.Add(SaveMe);
            db.SaveChanges();
        }

        public List<int> GetVideosByCourseId(int CourseId)
        {
            var all = GetAll();
            List<int> VideosIDs = new();
            foreach (var c in all)
            {
                if (c.CourseId == CourseId)
                {
                    VideosIDs.Add(c.VidId);
                }
            }
            return VideosIDs;

        }

        public List<CoursesNames> ToGetPdfsWithCourses(int Co_id)
        {
            var lop = db.coursesNames.ToList();
            var pdfs = new List<CoursesNames>();
            foreach (var courses in lop)
            {
                if (courses.CourseId == Co_id && courses.PdfId > 0)
                {
                    pdfs.Add(courses);
                }
            }
            // pdfs All pdf with courses
            return pdfs;
        }


    }
}
