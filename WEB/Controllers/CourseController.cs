using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WEB.Data;
using WEB.IRepo;
using WEB.Models;
using WEB.Repo;
using WEB.ViewModel;

namespace WEB.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        #region InJect
        private readonly ICreditCourses courseServes;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICoursesNamesBLL coursesNamesServices;
        private readonly IVideoBBL videoServices;
        private readonly IQuizBLL quizServices;
        private readonly IpdfBLL pdfService;
        private readonly IAccountBLL accountBLL;

        public CourseController(ICreditCourses courseServes, UserManager<ApplicationUser> userManager
            , ICoursesNamesBLL coursesNamesServices, IVideoBBL videoServices, IQuizBLL quizServices,
            IpdfBLL pdfService, IAccountBLL accountBLL)
        {
            this.courseServes = courseServes;
            this.userManager = userManager;
            this.coursesNamesServices = coursesNamesServices;
            this.videoServices = videoServices;
            this.quizServices = quizServices;
            this.pdfService = pdfService;
            this.accountBLL = accountBLL;
        }
        #endregion


        private static readonly object lockObject = new object();
        public IActionResult CoursesPage() // CousesssssPage
        {

            CreditCourses creditCourses = new CreditCourses();
            List<CreditCourses> courses = courseServes.GetAllCourses();
            return View((creditCourses, courses));
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]




        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]
        public IActionResult EditeCourses()
        {
            List<CreditCourses> courses = courseServes.GetAllCourses();
            return View(courses);
        }
        [Authorize(Roles = (Constans.roleAdmin))]





        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost]
        public async Task<IActionResult> EditeCourses(List<CreditCourses> Courses)
        {


            var parallelCourses = Courses.AsParallel();
            List<Task> processingTasks = new List<Task>();
            processingTasks.AddRange(parallelCourses.Select(ProcessCourseAsync));
            await Task.WhenAll(parallelCourses.Select(ProcessCourseAsync));
            var NewCourses=await courseServes.GetAllCoursesAsync();
            return View(NewCourses);
        }
        private async Task ProcessCourseAsync(CreditCourses course)
        {
            lock (lockObject)
            {
                var realCourse =  courseServes.GetCourseAsync(course.id);
                if (realCourse == null)
                {// If Course not founded
                    return;
                }
                if (course.img != null)
                {
                     courseServes.SaveCourseImge(course);
                }
                  courseServes.ChangeNamePriceAsync(course.id, course.name, course.Price);
            }
            //realCourse.name = course.name;
            //realCourse.Price = course.Price;
          // var end= await courseServes.SaveAsync();
            // Save Changes ! 
           // courseServes.Save();

        }

        #region Old ForLoop
        ////Forloop about it and rescan them !
        //foreach (var course in Courses)
        //{
        //	// Get imge Path First!
        //	//Service send to this iformFile and it getBath return String
        //	var RealCourse = courseServes.GetCourse(course.id);
        //	if (RealCourse == null)
        //	{
        //		return View("NotFound404", "Home");
        //	}
        //	if (course.img != null)
        //	{
        //		courseServes.SaveCourseImge(course);
        //	}
        //	RealCourse.name = course.name;
        //	RealCourse.Price = course.Price;
        //	courseServes.Save();
        //}
        //return View(Courses);
        #endregion

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult DeleteCourse(int id)
        {
            var c = courseServes.GetCourse(id);
            if (c == null)
            {
                return View("NotFound404", "Home");
            }
            courseServes.delete(c);
            List<CreditCourses> courses = courseServes.GetAllCourses();
            return View("EditeCourses", courses);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost]
        public IActionResult AddCourse(CreditCourses creditCourse)
        {
            CreditCourses creditCourses = new CreditCourses();
            List<CreditCourses> courses = courseServes.GetAllCourses();
            if (ModelState.IsValid)
            {
                courseServes.Add(creditCourse);
                ViewBag.Success = "Saving Done";
                return RedirectToAction("CoursesPage", (creditCourses, courses));
            }
            return RedirectToAction("CoursesPage", (creditCourses, courses));


        }

        [Authorize(Roles = (Constans.roleAdmin))]
        // Studen Ask
        public async Task<IActionResult> request(List<CreditCourses> courses)
        {
            var Uname = User.Identity.Name;
            var x = ModelState.IsValid;
            var user = await userManager.FindByNameAsync(Uname);
            if (user.degree != Alldegrees.آخري)
            {
                CreditCourses modeld = new CreditCourses();
                List<CreditCourses> model2d = courseServes.GetAllCourses();
                ViewBag.Error = "الرجاء تسجيل الدخول في خانه صف دراسي اخري";

                return View("CoursesPage", (modeld, model2d));
            }

            if (user == null)
            {
                return BadRequest();
            }

            foreach (var course in courses)
            {
                if (course.join == true)
                {
                    CoursesNames MyCourses = new()
                    {
                        CourseId = course.id,
                        Name = course.name,
                        UserId = user.Id
                    };
                    int courseid = MyCourses.CourseId;
                    var useridentifier = MyCourses.UserId;
                    var all = coursesNamesServices.GetAll();

                    var access = false;
                    foreach (var c in all)
                    {
                        if (c.CourseId == courseid && c.UserId == useridentifier && c.Access == true) // ALLLL?? 
                        {
                            access = true;
                        }
                    }
                    if (access == false)
                    {
                        coursesNamesServices.Add(MyCourses);
                    }



                    // course.join = true; // UI => To All !  بعدين 
                    ViewBag.request = "Your request sent..! ";
                }
            }

            CreditCourses model = new CreditCourses();
            List<CreditCourses> model2 = courseServes.GetAllCourses();
            return View("CoursesPage", (model, model2));


        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]
        public IActionResult OtherCoursesStudents()
        {

            var model = coursesNamesServices.GetAllForUsers();
            return View(model);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost] // Master Answer
        public async Task<IActionResult> OtherCoursesStudents(List<CoursesNames> courses)
        {
            foreach (var course in courses)
            {
                var user = await userManager.FindByIdAsync(course.UserId);
                if (course.Access) // Accept to Access ?
                {
                    var currentCourse = coursesNamesServices.GetById(course.Id);

                    currentCourse.Access = true;
                    coursesNamesServices.save();
                    user.joinCourse = true;
                    await userManager.UpdateAsync(user);
                }
            }
            return View(courses);
        }
        // ADD-Display Videos...
        [HttpGet]
        public async Task<IActionResult> CoursePage(int id)
        {
            #region  Constans
            var un = User.Identity.Name;
            var user = await userManager.FindByNameAsync(un);
            bool isAdmin = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
            var userCourses = coursesNamesServices.GetMyCoursesNames(user.Id);
            var CurrentCourse = coursesNamesServices.GetMyCourseName(id); // 
            var VideosIds = coursesNamesServices.GetVideosByCourseId(id);// vidIds Null
            var models = videoServices.videosByIds(VideosIds);
            var model = new AccessToVideo();
            ViewBag.id = id;

            #endregion
            if (isAdmin)
            {
                return
                    View((models, model)); // Single Video To Add (model => Videos & model+ Add)

                //return Content($"Welcome ==> {id} name ==> {CurrentCourse}");

            }
            //Checking For Can Enroll Or Not
            if (user.joinCourse)
            {

                if (userCourses != null)
                {
                    foreach (var AccessedCourses in userCourses)
                    {
                        if (AccessedCourses == CurrentCourse)
                        {
                            // Send Course Videos

                            ViewBag.id = id;
                            // Service videos which in VN same id and courseId // idVideos from NV == > VbyId

                            return View((models, model));
                            //return Content($"Welcome ==> {id} name ==> {CurrentCourse}");
                        }
                    }
                }
                return Content("NOT Access ");

            }
            else return Content("NOT Access ");
        }

        [HttpPost]
        public IActionResult CoursePage(AccessToVideo video, int id)
        {
            var VideosIds = coursesNamesServices.GetVideosByCourseId(id);
            var videos = videoServices.videosByIds(VideosIds);
            video = new AccessToVideo();
            ViewBag.id = id;

            return View((videos, video));
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult DeleteVid(string id)
        {
            var num_list = id.Split(',');

            var cid = int.Parse(num_list[0]);// Cid => Other
            var vidid = int.Parse(num_list[1]);

            videoServices.Delete(vidid);

            coursesNamesServices.DeletVidFromCourses(vidid, cid);


            var VideosIds = coursesNamesServices.GetVideosByCourseId(cid);// vidIds Null
            var models = videoServices.videosByIds(VideosIds);
            var model = new AccessToVideo();
            ViewBag.id = cid;

            return View("CoursePage", (models, model));
        }
        // ADd Vid
        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost]
        public IActionResult AddVideo(AccessToVideo Video)
        {// Catsh Vid And Course ID ! 
            int cId = Video.Id;
            Video.Id = 0;
            videoServices.SavingVideoWithCourse(Video, cId);
            return RedirectToAction("CoursePage", new { id = cId });
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]
        public IActionResult AddPdf(int id)
        {
            ViewBag.id = id;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = (Constans.roleAdmin))]
        public async Task<IActionResult> AddPdf(PdfMaterial pdf)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return View("NotFound404", "Home"); }

            //Only Admin
            var result = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
            if (result)
            {


                int id = pdf.Id;
                pdf.Id = 0;
                pdfService.SavePdfWithCourses(pdf, id);
                ViewBag.Suc = "Done !";
                return View();
            }
            else
            {
                return View("NotFound404", "Home");
            }
        }


        public IActionResult DisplayPdfs(int id) //uq // CourseId
        {//Date?
         // CreditCourseId => CourseNames --> C==id + pdf>1
         // var UQ = quizServices.GetQuizUser(id);

            var pdfs = coursesNamesServices.ToGetPdfsWithCourses(id);
            var listPdfs = new List<PdfMaterial>();
            foreach (var item in pdfs)
            {
                var addme = pdfService.GetPdfById(item.PdfId);
                listPdfs.Add(addme);

            }
            ViewBag.id = id;
            // PDF?
            return View(listPdfs.OrderByDescending(x => x.Date).ToList()); // List OF PDFS 
        }

        public IActionResult DisplayPdf(int id) //pdf id 20
        { ////// HERE Error of studentFile
            var x = pdfService.GetPdfById(id);


            // PDF?
            return View(x);
        }

        public IActionResult FilePdf(int id)
        {
            var mod = pdfService.GetPdfById(id);
            return View(mod);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]
        public async Task<IActionResult> AddQuiz(int id) // Course Id
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return View("NotFound404", "Home"); }

            //Only Admin
            var result = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
            if (result)
            {


                ViewBag.id = id;
                return View();
            }
            else
            {
                return View("NotFound404", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = (Constans.roleAdmin))]
        public ActionResult AddQuiz(QuizTable quiz, int id)
        {
            quiz.Id = 0;
            quizServices.CreatQuizWithCourse(quiz, id);
            quizServices.Save();
            return View();
        }


        public IActionResult DisplayQuizes(int id) // EDit View !! 
        {
            // Quizes + Table => UserQuizes ! 
            ViewBag.id = id; // Course Id

            var userQuizes = quizServices.ShowCreditCoursesStudents();
            var Quezies = coursesNamesServices.GetAllForQuiz(id); //C_id , Tolist=>CN
            return View((Quezies, userQuizes));
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult DeleteQuiz(string id)
        {
            var num_list = id.Split(',');
            var cid = int.Parse(num_list[0]);// Cid => Other
            var CourseNamesid = int.Parse(num_list[1]);
            var cn = coursesNamesServices.GetById(CourseNamesid);
            ViewBag.id = cn.CourseId;
            quizServices.Delete(cn.QuizId);
            coursesNamesServices.DeletQuizFromCourses(cn.QuizId, cid);

            var userQuizes = quizServices.ShowCreditCoursesStudents();
            var Quezies = coursesNamesServices.GetAllForQuiz(cid); //C_id , Tolist=>CN

            return View("DisplayQuizes", (Quezies, userQuizes));


        }


        public IActionResult ToTakeQuiz(int id, int ids) // CN 
                                                         // VM To Send 2 ids?
        {

            DateTime startTime = DateTime.Now;
            HttpContext.Session.Set("StartTime", Encoding.UTF8.GetBytes(startTime.ToString()));

            // Calculate the end time of the exam (e.g., 1 hour time limit)
            DateTime endTime = startTime.AddSeconds(40);
            ViewBag.EndTime = endTime;
            // Check Degree
            // CN
            var cn = coursesNamesServices.GetById(id);
            ViewBag.Courseid = cn.CourseId;
            QuizTable quiz = quizServices.GetQuizByIdwithQO(cn.QuizId);
            if (quiz.FileNameMaster != null)
            {
                var pdfName = quiz.FileNameMaster;
                // string uploadedFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets", "images", "Quiz");
                // string filePath = Path.Combine(uploadedFolder, pdfName);
                string pdfUrl = "/assets/images/Quiz/" + pdfName;

                // Pass the pdfUrl to the view
                ViewBag.PDF = pdfUrl;
                ViewBag.Id = ids;
            }
            ViewBag.Id = ids;
            //  _context.QuizsTable.Include(q => q.Questions).ThenInclude(q => q.Options).FirstOrDefault(q => q.Id == id);
            return View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(int quizId, List<int> selectedAnswers, int id) //Course Id
        {
            var name = User.Identity.Name;
            var CurrentUser = await userManager.FindByNameAsync(name);
            QuizTable quiz = quizServices.GetQuizByIdwithQ(quizId);
            int score = 0;
            for (int i = 0; i < quiz.Questions.Count; i++)
            {

                if (selectedAnswers[i] == (quiz.Questions[i].CorrectAnswer - 1))
                {
                    score++;
                }
            }
            var x = quiz.Id;
            List<Question> Question = quizServices.GetAllQuestions(x); // want to edit this to Get only quiz questions!!
                                                                       // Question.RemoveAll();

            ViewBag.Count = Question.Count;
            ViewBag.Q = Question;
            ViewBag.U = User.Identity.Name;
            var O = quizServices.GetAllOptions(); // Edit This to Get Only Quiz Options
            ViewBag.Score = score;
            quizServices.AddingUE(CurrentUser.Id, quiz.Id, CurrentUser.UserName, quiz.Title, id, score, quiz.Questions.Count, quiz.Semester);
            quizServices.Save();
            var Answers = new List<string>();
            foreach (var Q in Question)
            {
                var alloption = new List<string>();

                var k = @Q.Text; // Question
                var Delete = @Q.CorrectAnswer;
                var y = @Q.CorrectAnswer - 1; // Number of Soluation?

                for (var option = 0; option < O.Count; option++)
                {
                    if (O[option].QuestionId == Q.Id) // Options To Question
                    {
                        alloption.Add(O[option].Text);
                    }
                    else
                    {
                        continue;
                    }
                }
                for (var ans = 0; ans < alloption.Count; ans++)
                {
                    if (ans == @Q.CorrectAnswer - 1)
                    {
                        Answers.Add(alloption[ans]);
                    }
                }
            }
            return View(Answers);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        [HttpGet]
        public IActionResult AddWreiiterQuiz(int id)
        {
            ViewBag.Id = id;
            QuizTable mode = new();
            return View(mode);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult AddWreiiterQuiz(QuizTable quiz, int Id) // id=> Course Id
        {
            quizServices.AddQuizPdfWithCourse(quiz, Id);
            ViewBag.Suc = " Done..! ";
            QuizTable mode = new();

            return View(mode);
        }

        public async Task<IActionResult> SubmitPDFQuiz(QuizTable quiz) // q.id=0 , id=> CN --> Course ID => UQ
        {

            var cn = coursesNamesServices.GetById(quiz.Id); // Edit Service
            ViewBag.id = cn.CourseId;
            //   var Quezies = coursesNamesServices.GetAllForQuiz(cn.CourseId); //C_id , Tolist=>CN

            quiz.Id = 0;

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            cn.UserId = user.Id;
            //  var Quiz=quizServices.GetQuizByIdwithQ(cn.QuizId);



            quizServices.SaveStudentPdfWithCourses(quiz.File, user, cn.QuizId, cn.CourseId);

            quiz.Id = cn.Id;
            // Course Id
            return RedirectToAction("index", "Home");


        }

        [HttpGet]
        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult Correcting()
        {
            var model = quizServices.WrittenFilterationToCourses(quizServices.Showing1());
            // userQ
            return View(model);
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult Correcting(List<UsersQuiz> correctquiz)
        {
            foreach (var tab in correctquiz)
            {
                // Do it in Reposatory ! 

                var Quiz = quizServices.GetQuizUser(tab.Id);
                Quiz.ExamDegree = tab.ExamDegree;
                Quiz.MaxDegree = tab.MaxDegree;
                Quiz.Makalie = true;
                quizServices.Save();


            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult DeleteStd(int id)
        {
            // Handel CourseNames And User 
            // IAccount Ser
            accountBLL.DeleteCreditCourseStd(id);
            //	var model = coursesNamesServices.GetAllForUsers();
            //return View("OtherCoursesStudents", model);
            return RedirectToAction("index", "Home");
        }

        [Authorize(Roles = (Constans.roleAdmin))]
        public IActionResult DeletePdf(string id) // Cid and return to Display pdfs
        {//3,16
            var num_list = id.Split(',');

            var cid = int.Parse(num_list[0]);// Cid => Other
            var pdfid = int.Parse(num_list[1]);


            pdfService.DeletePdf(pdfid);
            coursesNamesServices.DeletPdfFromCourses(pdfid, cid);

            var pdfs = coursesNamesServices.ToGetPdfsWithCourses(cid);
            var listPdfs = new List<PdfMaterial>();
            foreach (var item in pdfs)
            {
                var addme = pdfService.GetPdfById(item.PdfId);
                listPdfs.Add(addme);

            }
            ViewBag.id = cid;
            return View("DisplayPdfs", listPdfs);
        }


    }
}
