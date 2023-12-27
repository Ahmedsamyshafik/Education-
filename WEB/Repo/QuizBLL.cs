using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
    public class QuizBLL : IQuizBLL
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICoursesNamesBLL cS;
        private readonly ICreditCourses oS;

        public QuizBLL(ApplicationContext db, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager
            , ICoursesNamesBLL CS, ICreditCourses OS)
        {
            this.db = db;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            cS = CS;
            oS = OS;
        }

        public List<Option> GetAllOptions()
        {
            return db.Options.ToList();
        }

        public List<Question> GetAllQuestions(int Qid)
        {
            return db.Questions.Where(x => x.QuizTableId == Qid).ToList();
        }

        // Delete => Delete From UsersQuiz

        public List<QuizTable> GetAllquizes()
        {
            return db.QuizsTable.ToList()
        .OrderBy(x => x.Date)
        .ToList();
            // DateTime And 
        }

        public List<QuizTable> GetAllquizesForAdmin1()
        {
            return db.QuizsTable.Where(q => q.Semester == Semester.الأول).ToList();

        }

        public List<QuizTable> GetAllquizesForAdmin2()
        {
            return db.QuizsTable.Where(q => q.Semester == Semester.الثاني).ToList();

        }

        public QuizTable GetQuizByIdwithQ(int id)
        {
            return db.QuizsTable.Include(q => q.Questions).FirstOrDefault(q => q.Id == id);
        }

        public QuizTable GetQuizByIdwithQO(int id)
        {
            return db.QuizsTable.Include(q => q.Questions).ThenInclude(q => q.Options).FirstOrDefault(q => q.Id == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async void SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public QuizTable GetQuiz(int id)
        {
            return db.QuizsTable.FirstOrDefault(x => x.Id == id);
        }

        public UsersQuiz GetQuizUser(int id)
        {
            return db.UsersQuiz.FirstOrDefault(x => x.Id == id);
        }

        public async void AddingUE_ASYNC(string Userid, int ExamId, string userName, string ExamName, int ExamDegree, int MaxDegree, Semester semester)
        {
            UsersQuiz user = new UsersQuiz()
            {
                UserId = Userid,
                ExamId = ExamId,
                UserName = userName,
                ExamName = ExamName,
                ExamDegree = ExamDegree,
                MaxDegree = MaxDegree,
                semester = semester
            };


            await db.UsersQuiz.AddAsync(user);
            // SaveAsync();
            //   db.UsersExamsM.
        }

        public async void AddAsync(QuizTable qiz)
        {
            await db.AddAsync(qiz);
        }

        public async void AddAsyncUserQuiz(UsersQuiz quiz)
        {
            await db.AddAsync(quiz);
        }

        #region Display Courses 
        public List<UsersQuiz> showing()
        {
            return db.UsersQuiz.ToList();
        }
        public List<UsersQuiz> Showing1()
        {
          //  var test = db.UsersQuiz.ToList();

            return db.UsersQuiz.Where(u => u.semester == Semester.الأول).ToList();

        }

        public List<UsersQuiz> Showing2()
        {
            return db.UsersQuiz.Where(u => u.semester == Semester.الثاني).ToList();

        }

		public List<UsersQuiz> ShowCreditCoursesStudents()
		{
			return db.UsersQuiz.Where(u => u.CourseId >0).ToList();

		}
        public void DeleteRangeOdUserQuiz(List<UsersQuiz> quiz) { 
        db.RemoveRange(quiz);
            db.SaveChanges();
        }

		public List<UsersQuiz> GetAllDataToUser(string UserID)
        {
            List<UsersQuiz> all=new List<UsersQuiz>();
            var allTolist = showing();
            foreach (var user in allTolist) {
                if (user.UserId == UserID.ToString())
                {
                    all.Add(user); 
                }
            }
			return all;


		}

		public List<QuizTable> GetAllquizesForOne1()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الأول" && course.Semester.ToString() == "الأول")
                {
                    ActulyCourses.Add(course);
                }
            }
           // var AcC = ActulyCourses.OrderBy(x => x.Date).ToList();
            return ActulyCourses.OrderByDescending(x => x.Date).ToList(); 

        }

        public List<QuizTable> GetAllquizesForTwo1()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الثاني" && course.Semester.ToString() == "الأول")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public List<QuizTable> GetAllquizesForThree1()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الثالث" && course.Semester.ToString() == "الأول")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public List<QuizTable> GetAllquizesForFour1()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "آخري" && course.Semester.ToString() == "الأول")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public List<QuizTable> GetAllquizesForOne2()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الأول" && course.Semester.ToString() == "الثاني")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();

		}

		public List<QuizTable> GetAllquizesForTwo2()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الثاني" && course.Semester.ToString() == "الثاني")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public List<QuizTable> GetAllquizesForThree2()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "الثالث" && course.Semester.ToString() == "الثاني")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public List<QuizTable> GetAllquizesForFour2()
        {
            var Courses = GetAllquizes();
            List<QuizTable> ActulyCourses = new List<QuizTable>(); // Initialize the list
            foreach (var course in Courses)
            {
                if (course.Alldegrees.ToString() == "آخري" && course.Semester.ToString() == "الثاني")
                {
                    ActulyCourses.Add(course);
                }
            }
			return ActulyCourses.OrderByDescending(x => x.Date).ToList();
		}

		public int getCount()
        {
            return db.UsersQuiz.Count();
        }

        #endregion

        public void Delete(int id)
        {
            var Qz = GetQuiz(id);
            db.Remove(Qz);
            Delete2(Qz.Title);
            db.SaveChanges();

        }

        public async void SaveStudentPdf(IFormFile form, ApplicationUser user, int QuizId) //qid
        {   
            var The_Quiz = GetQuiz(QuizId);

            var ConsPdfPass = "/assets/Images/QuizStudent";
            var PdfPath = $"{hostingEnvironment.WebRootPath}{ConsPdfPass}";
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + form.FileName;


			string filePath = Path.Combine(ConsPdfPass, uniqueFileName);

			AddingUE_WithoutDegrees(user.Id, The_Quiz.Id, user.UserName, The_Quiz.Title, The_Quiz.Semester, filePath);

			 filePath = Path.Combine(PdfPath, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
			await	form.CopyToAsync(fileStream);
			}
		}




        public  void SaveStudentPdfWithCourses(IFormFile form, ApplicationUser user, int QuizId, int CourseId) //qid
        {
            var The_Quiz = GetQuiz(QuizId);

            var ConsPdfPass = "/assets/Images/QuizStudent";
            var PdfPath = $"{hostingEnvironment.WebRootPath}{ConsPdfPass}";
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + form.FileName;
			string filePath = Path.Combine(ConsPdfPass, uniqueFileName);

            AddingUECourses_WithoutDegrees(user.Id, The_Quiz.Id, user.UserName, CourseId, The_Quiz.Title, The_Quiz.Semester, filePath);
			 filePath = Path.Combine(PdfPath, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                form.CopyTo(fileStream);
            }

           
        }


        public void AddingUECourses_WithoutDegrees(string Userid, int ExamId, string userName, int CourseId,string ExamName, Semester semester, string filename)
        {
            UsersQuiz user = new UsersQuiz()
            {
                UserId = Userid,
                ExamId = ExamId,
                UserName = userName,
                ExamName = ExamName,
                StudentFile = filename,
                semester = semester,
                Makalie = true,
                CourseId = CourseId
            };

            db.UsersQuiz.Add(user);
            Save();
        }


        public void Add(QuizTable quiz)
        {
            db.QuizsTable.Add(quiz);
            Save();
        }

        public void AddingUE(string Userid, int ExamId, string userName, string ExamName, int CourseID, int ExamDegree, int MaxDegree, Semester semester)
        {
            UsersQuiz user = new UsersQuiz()
            {
                UserId = Userid,
                ExamId = ExamId,
                UserName = userName,
                ExamName = ExamName,
                ExamDegree = ExamDegree,
                MaxDegree = MaxDegree,
                semester = semester,
                CourseId=CourseID
            };


            db.UsersQuiz.Add(user);
            Save();
        }

        public void AddingUE_WithoutDegrees(string Userid, int ExamId, string userName, string ExamName, Semester semester, string filename)
        {
            UsersQuiz user = new UsersQuiz()
            {
                UserId = Userid,
                ExamId = ExamId,
                UserName = userName,
                ExamName = ExamName,
                StudentFile = filename,
                semester = semester,
                Makalie = true
            };


            db.UsersQuiz.Add(user);
            Save();
        }

        public async void AddingUE_Async(string Userid, int ExamId, string userName, string ExamName, int ExamDegree, int MaxDegree, Semester semester)
        {
            UsersQuiz user = new UsersQuiz()
            {
                UserId = Userid,
                ExamId = ExamId,
                UserName = userName,
                ExamName = ExamName,
                ExamDegree = ExamDegree,
                MaxDegree = MaxDegree,
                semester = semester,
                Makalie = true
            };


            await db.UsersQuiz.AddAsync(user);
            SaveAsync();
        }

        public List<UsersQuiz> WrittenFilteration(List<UsersQuiz> usersQuizs)
        {
            List<UsersQuiz> filteration = new();
            foreach (var usersQuiz in usersQuizs)
            {
                if (usersQuiz.Makalie == true)
                {
                    filteration.Add(usersQuiz);
                }
            }
            return filteration;
        }
        public List<UsersQuiz> WrittenFilterationToCourses(List<UsersQuiz> usersQuizs)
        {
            List<UsersQuiz> filteration = new();
            foreach (var usersQuiz in usersQuizs)
            {
                if (usersQuiz.Makalie == true && usersQuiz.CourseId>0)
                {
                    filteration.Add(usersQuiz);
                }
            }
            return filteration;
        }

        public void CreatQuizWithCourse(QuizTable quiz, int C_Id)
        {
            db.Add(quiz);
            db.SaveChanges();
            var Course = oS.GetCourse(C_Id);

            CoursesNames coursesNames = new()
            {
                //UserId=string.Empty,
                CourseId = Course.id,
                Name = quiz.Title,
                VidId = 0,
                PdfId = 0,
                QuizId = quiz.Id,




            };
            cS.Add(coursesNames);
            //  oS.AddToMyCourses(coursesNames);
        }

		public async void AddQuizPdfWithCourse(QuizTable quiz, int CId)
		{
			string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "images", "Quiz");

			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}

			string uniqueFileName = Guid.NewGuid().ToString() + "_" + quiz.File.FileName;
			var SaveMe = new QuizTable()
			{
				Alldegrees = quiz.Alldegrees,
				Semester = quiz.Semester,
				Description = quiz.Description,
				Title = quiz.Title,
				FileNameMaster = uniqueFileName,
				Date = DateTime.Now,
				//    IsWritten = true;
			};

			db.QuizsTable.Add(SaveMe);
			Save();
			CoursesNames coursesNames = new()
			{
				Access = SaveMe.Access,
				CourseId = CId,
				Name = SaveMe.Title,
				QuizId = SaveMe.Id
			};
			db.Add(coursesNames);
			db.SaveChanges();
			// Add To CN ?

			string filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await quiz.File.CopyToAsync(fileStream);
			}
		}

        public void DeleteFromQuizTable(int id)
        {
           var D= GetQuizUser(id);
            if (D != null)
            {
                db.UsersQuiz.Remove(D);
                db.SaveChanges();
            }
        }


		public async void AddPdf(QuizTable quiz)
        {
            //Saving pdf
            //  db.QuizsTable.Add(quiz);
            // Saving in Server..
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "images", "Quiz");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + quiz.File.FileName;
            var SaveMe = new QuizTable()
            {
                Alldegrees = quiz.Alldegrees,
                Semester = quiz.Semester,
                Description = quiz.Description,
                Title = quiz.Title,
                FileNameMaster = uniqueFileName,
                //    IsWritten = true;

            };
            db.QuizsTable.Add(SaveMe);
            Save();
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await quiz.File.CopyToAsync(fileStream);
            }
        }


        // Delete from UserQuiz when Delte Quiz! 
        public void Delete2(string Exname)
        {
            var lop = db.UsersQuiz.ToList();
            foreach (var user in lop)
            {
                if (user.ExamName == Exname)
                {
                    db.UsersQuiz.Remove(user);
                }
            }
        }


    }
}
