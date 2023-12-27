using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Controllers
{
	[Authorize]
	public class QuizController : Controller
	{
		#region inJect
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IQuizBLL quizServes;

		//CTOR
		public QuizController
		//ApplicationContext context,
		(
		UserManager<ApplicationUser> _userManager,
		SignInManager<ApplicationUser> _signInManager,
		IQuizBLL _QuizServes,
		RoleManager<IdentityRole> _roleManager,
		IWebHostEnvironment hostingEnvironment
			)
		{
			//  _context = context;
			userManager = _userManager;
			signInManager = _signInManager;
			quizServes = _QuizServes;
			roleManager = _roleManager;
			this.hostingEnvironment = hostingEnvironment;
		}
		#endregion

		public async Task<IActionResult> Index(int id)
		{
			#region Allowing Access 
			var curId = 0;
			var username = User.Identity.Name;
			if (string.IsNullOrEmpty(username))
			{
                return View("NotFound404", "Home");
            }
			var user = await userManager.FindByNameAsync(username);
			if (user == null) // ||
			{

                return View("NotFound404", "Home");
            }
			if (await userManager.IsInRoleAsync(user, Constans.roleAdmin))
			{
				curId = 4;

			}
			else
			{
				if (user.IsInRole == false)
				{
					return RedirectToAction("Index", "Home");
				}
				// if id != user.degree => Noo
				if (user.degree == Alldegrees.الأول) { curId = 1; }
				if (user.degree == Alldegrees.الثاني) { curId = 2; }
				if (user.degree == Alldegrees.الثالث) { curId = 3; }

				if (user.degree == Alldegrees.آخري) { return RedirectToAction("index", "home"); }

			}
			if (curId != id && curId != 4)
			{
				return RedirectToAction("Index", "Home");

			}
			#endregion
			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();
			// No Admin in Table
			var Usersquiz2 = new List<UsersQuiz>();

			foreach (var us in Usersquiz)
			{
				var usr = await userManager.FindByIdAsync(us.UserId);
				var Rol = await userManager.IsInRoleAsync(usr, Constans.roleAdmin);
				if (usr == null || Rol) { continue; }
				Usersquiz2.Add(us);
			}
			if (id == 1) { quizzes = quizServes.GetAllquizesForOne1(); }
			if (id == 2) { quizzes = quizServes.GetAllquizesForTwo1(); }
			if (id == 3) { quizzes = quizServes.GetAllquizesForThree1(); }

			ViewBag.id = id; // may not? 
			return View((quizzes, Usersquiz2));
		}

		public async Task<IActionResult> Index2(int id)
		{
			#region Allowing Access 
			var curId = 0;
			var username = User.Identity.Name;
			if (string.IsNullOrEmpty(username))
			{
                return View("NotFound404", "Home");
            }
			var user = await userManager.FindByNameAsync(username);
			if (user == null) // ||
			{

                return View("NotFound404", "Home");
            }
			if (await userManager.IsInRoleAsync(user, Constans.roleAdmin))
			{
				curId = 4;

			}
			else
			{
				if (user.IsInRole == false)
				{
					return RedirectToAction("Index", "Home");
				}
				// if id != user.degree => Noo
				if (user.degree == Alldegrees.الأول) { curId = 1; }
				if (user.degree == Alldegrees.الثاني) { curId = 2; }
				if (user.degree == Alldegrees.الثالث) { curId = 3; }

				if (user.degree == Alldegrees.آخري) { return RedirectToAction("index", "home"); }

			}
			if (curId != id && curId != 4)
			{
				return RedirectToAction("Index", "Home");

			}
			#endregion
			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();
			// No Admin in Table
			var Usersquiz2 = new List<UsersQuiz>();

			foreach (var us in Usersquiz)
			{
				var usr = await userManager.FindByIdAsync(us.UserId);
				var Rol = await userManager.IsInRoleAsync(usr, Constans.roleAdmin);
				if (usr == null || Rol) { continue; }
				Usersquiz2.Add(us);
			}

			if (id == 1) { quizzes = quizServes.GetAllquizesForOne2(); }
			if (id == 2) { quizzes = quizServes.GetAllquizesForTwo2(); }
			if (id == 3) { quizzes = quizServes.GetAllquizesForThree1(); }

			ViewBag.id = id; // may not? 
			return View((quizzes, Usersquiz2));
		}

		[HttpGet]
		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult AddQuiz()
		{
			return View();
		}

		[Authorize(Roles = (Constans.roleAdmin))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddQuiz(QuizTable quiz)
		{
			quizServes.Add(quiz);
			quizServes.Save();
			return View();
		}


		[Authorize(Roles = (Constans.roleAdmin))]
		[HttpGet]
		public IActionResult AddWrittenQuiz()
		{
			return View();
		}

		[Authorize(Roles = (Constans.roleAdmin))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddWrittenQuiz(QuizTable quiz)
		{
			quizServes.AddPdf(quiz);
			ViewBag.QuizSuc = "Saved Done! ";
			return View();
		}

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult QuizAccess(List<QuizTable> quizes)
		{
			var deg = Alldegrees.الأول;
			var sem = Semester.الثاني;
			foreach (var quizI in quizes)
			{
				var Q = quizServes.GetQuizByIdwithQ(quizI.Id);
				deg = Q.Alldegrees;
				sem = Q.Semester;
				if (quizI.Access)
				{
					Q.Access = true;
					quizI.Access = true;
					quizServes.Save();
				}
				else
				{
					Q.Access = false;
					quizI.Access = false;
					quizServes.Save();
				}
			}
			var quiz = new QuizTable(); // Temp 
			List<QuizTable> quizzes = new List<QuizTable>(); // Semester
			if (sem == Semester.الأول)
			{
				if (deg == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForOne1(); }
				if (deg == Alldegrees.الثاني) { quizzes = quizServes.GetAllquizesForTwo1(); }
				if (deg == Alldegrees.الثالث) { quizzes = quizServes.GetAllquizesForThree1(); }
			}
			else
			{
				if (deg == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForOne2(); }
				if (deg == Alldegrees.الثاني) { quizzes = quizServes.GetAllquizesForTwo2(); }
				if (deg == Alldegrees.الثالث) { quizzes = quizServes.GetAllquizesForThree2(); }
			}
			var q = quizServes.GetAllquizes();
			return RedirectToAction("index", new { quiz = quiz, quizes = quizes });
		}


		public async Task<IActionResult> TakeQuizExam(int id) // QuizTable id
		{           // show Exam 
					//Timer

			//if user take quiz
			var user =await userManager.FindByNameAsync(User.Identity.Name);
			if (user == null) { return View("NotFound404", "Home"); }
			var all= quizServes.Showing1();
			var UserRole = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
			foreach(var u in all)
			{
				if(u.UserId==user.Id && u.ExamId == id &&UserRole!=true)// اخد الامتحان وطالب عادي
				{
					return RedirectToAction("NoExam","Home");
				}
			}
			DateTime startTime = DateTime.Now;
			//HttpContext.Session.Set("StartTime", Encoding.UTF8.GetBytes(startTime.ToString()));
			DateTime endTime = startTime.AddSeconds(40); //Var
			ViewBag.EndTime = endTime;

			QuizTable quiz = quizServes.GetQuizByIdwithQO(id);

			string name = User.Identity.Name;
			if (name != null && name.Length > 0)
			{
				//var user = await userManager.FindByNameAsync(name);
				if (user != null)
				{
					var YesOrNo = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
					// Access Yes Or Access=True
					if (YesOrNo || quiz.Access)
					{
						if (quiz.FileNameMaster != null) // Written 
						{
							var pdfName = quiz.FileNameMaster;
							string pdfUrl = "/assets/images/Quiz/" + pdfName;

							// Pass the pdfUrl to the view
							ViewBag.PDF = pdfUrl;
							ViewBag.Id = id;
						}
						return View(quiz);
					}
				}
			}

			return View("NotFound404", "Home");


		}

		[HttpPost]
		public async Task<IActionResult> SubmitQuiz(int quizId, List<int> selectedAnswers)
		{
			var name = User.Identity.Name;
			var CurrentUser = await userManager.FindByNameAsync(name);
			if (CurrentUser == null)
			{
				return View("NotFound404", "Home");
			}
			QuizTable quiz = quizServes.GetQuizByIdwithQ(quizId);
			int score = 0;
			for (int i = 0; i < quiz.Questions.Count; i++)
			{

				if (selectedAnswers[i] == (quiz.Questions[i].CorrectAnswer - 1))
				{
					score++;
				}
			}
			var x = quiz.Id;
			List<Question> Question = quizServes.GetAllQuestions(x);
			ViewBag.Count = Question.Count;
			ViewBag.Q = Question;
			ViewBag.U = User.Identity.Name;
			var O = quizServes.GetAllOptions();
			ViewBag.Score = score;
			quizServes.AddingUE(CurrentUser.Id, quiz.Id, CurrentUser.UserName, quiz.Title, 0, score, quiz.Questions.Count, quiz.Semester);
			quizServes.Save();
			var Answers = new List<string>();
			//Getting Correct Answer
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
			return View(Answers); // To Display Score  Q&A
		}

		[HttpPost]
		public async Task<IActionResult> SubmitPDFQuiz(QuizTable quiz)// cn => user , quiz  
		{

			var username = User.Identity.Name;
			var user = await userManager.FindByNameAsync(username);
			if (user == null)
			{
                return View("NotFound404", "Home");
            }
			var getQuiz = quizServes.GetQuizByIdwithQ(quiz.Id);

			quizServes.SaveStudentPdf(quiz.File, user, quiz.Id);


			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();
			if (getQuiz.Alldegrees == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForOne1(); }
			if (getQuiz.Alldegrees == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForTwo1(); }
			if (getQuiz.Alldegrees == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForThree1(); }

			// ViewBag.id = id; // may not? 
			return View("Index", (quizzes, Usersquiz));
		}

		
		[HttpGet]
		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult Correcting()
		{
			var model = quizServes.WrittenFilteration(quizServes.Showing1());

			return View(model);
		}

		public IActionResult DisplayPdf(int id)
		{


			var mod = quizServes.GetQuizUser(id);

			return View(mod);
		}
		[HttpPost]
		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult Correcting(List<UsersQuiz> correctquiz)
		{
			foreach (var tab in correctquiz)
			{
				// Do it in Reposatory ! 

				var Quiz = quizServes.GetQuizUser(tab.Id);
				Quiz.ExamDegree = tab.ExamDegree;
				Quiz.MaxDegree = tab.MaxDegree;
				Quiz.Makalie = true;
				quizServes.Save();


			}

			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();

			quizzes = quizServes.GetAllquizesForOne1();


			//ViewBag.id = id; // may not? 
			return View("index", (quizzes, Usersquiz));
		}

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult DeleteUserQuiz(int id)
		{
			quizServes.DeleteFromQuizTable(id);
			var QU = quizServes.GetQuizUser(id);
			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();
			quizzes = quizServes.GetAllquizesForOne1();
			ViewBag.id = 1; // may not? 
			return View("index", (quizzes, Usersquiz));
		}
		
		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult DeleteQuiz(int id)
		{
			if (id == 0)
			{
				return View("NotFound404", "Home");
			}
			var quz = quizServes.GetQuizByIdwithQ(id);
			var Deg = (quz).Alldegrees;
			quizServes.Delete(id);

			// index =
			List<QuizTable> quizzes = new List<QuizTable>();
			var Usersquiz = quizServes.Showing1();

			if (quz.Semester == Semester.الأول && quz.Alldegrees == Alldegrees.الأول) { id = 1; quizzes = quizServes.GetAllquizesForOne1(); }
			if (quz.Semester == Semester.الثاني && quz.Alldegrees == Alldegrees.الأول) { id = 1; quizzes = quizServes.GetAllquizesForOne2(); }
			if (quz.Semester == Semester.الأول && quz.Alldegrees == Alldegrees.الثاني) { id = 2; quizzes = quizServes.GetAllquizesForTwo1(); }
			if (quz.Semester == Semester.الثاني && quz.Alldegrees == Alldegrees.الثاني) { id = 2; quizzes = quizServes.GetAllquizesForTwo2(); }
			if (quz.Alldegrees == Alldegrees.الثالث) { id = 3; quizzes = quizServes.GetAllquizesForThree1(); }


			ViewBag.id = id; // may not? 
			return View("index", (quizzes, Usersquiz));

		}


	}
}
