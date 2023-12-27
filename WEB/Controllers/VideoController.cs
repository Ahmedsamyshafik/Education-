using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        #region InJect 
        private readonly IVideoBBL videoService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFreeCoursrsBLL freeCoursrs;

        public VideoController(IVideoBBL videoService, UserManager<ApplicationUser> userManager,
           IFreeCoursrsBLL freeCoursrs)
        {
            this.videoService = videoService;
            this.userManager = userManager;
            this.freeCoursrs = freeCoursrs;
        }
        #endregion

        // Only Students Degrees ..
        //Access To Videos ! 
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

            AccessToVideo vid = new AccessToVideo();

            List<AccessToVideo> videos = new(); // ToList! 
            if (id == 1) { videos = videoService.GetVideosForOne1(); }
            if (id == 2) { videos = videoService.GetVideosForTwo1(); }
            if (id == 3) { videos = videoService.GetVideosForThree1(); }

            ViewBag.id = id;
            return View((vid, videos));
        }

        public IActionResult Index2(int id)
        {
            AccessToVideo vid = new AccessToVideo();

            List<AccessToVideo> videos = new(); // ToList! 
            if (id == 1) { videos = videoService.GetVideosForOne2(); }
            if (id == 2) { videos = videoService.GetVideosForTwo2(); }
            if (id == 3) { videos = videoService.GetVideosForThree2(); }

            ViewBag.id = id;
            return View((vid, videos));
        }

        [HttpPost]
		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult AddVideo(AccessToVideo video)
        {
            if (ModelState.IsValid)
            {
                videoService.SavingVideo(video);
                ViewBag.vidSuccess = "Video Saved";
            }
            List<AccessToVideo> videos = videoService.GetAllVideos(); ;// momken abden ?

            var Deg = video.Degree;
            int x = 0;
            if (Deg == Alldegrees.الأول) { videos = videoService.GetVideosForOne1(); x = 1; }
            if (Deg == Alldegrees.الثاني) { videos = videoService.GetVideosForTwo1(); x = 2; }
            if (Deg == Alldegrees.الثالث) { videos = videoService.GetVideosForThree1(); x = 3; }


            // When Return
            AccessToVideo vid = new AccessToVideo();
            return RedirectToAction("Index", new { id = x, vid = vid, videos = videos }); ;
        }

		[Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost]
		public async Task<IActionResult> VideosAccess(List<AccessToVideo> videos)
        {
            var deg = Alldegrees.الأول;
            var sem = Semester.الثاني;
            foreach (var Vid in videos)
            {

                var Q = videoService.GetVideoById(Vid.Id);
                deg = Q.Degree;
                sem = Q.Semester;
                if (Vid.Access)
                {
                    Q.Access = true;
                    Vid.Access = true;
                    videoService.Save();
                }
                else
                {
                    Q.Access = false;
                    Vid.Access = false;
                    videoService.Save();
                }
            }
            AccessToVideo Model1 = new AccessToVideo();
            List<AccessToVideo> Model2 = new(); // ToList! 
            if (sem == Semester.الأول)
            {

                if (deg == Alldegrees.الأول) { Model2 = videoService.GetVideosForOne1(); }
                if (deg == Alldegrees.الثاني) { Model2 = videoService.GetVideosForTwo1(); }
                if (deg == Alldegrees.الثالث) { Model2 = videoService.GetVideosForThree1(); }
            }
            else
            {
                if (deg == Alldegrees.الأول) { Model2 = videoService.GetVideosForOne2(); }
                if (deg == Alldegrees.الثاني) { Model2 = videoService.GetVideosForTwo2(); }
                if (deg == Alldegrees.الثالث) { Model2 = videoService.GetVideosForThree2(); }
            }
            var q = videoService.GetAllVideos();
            return RedirectToAction("index", new { quiz = Model1, quizes = Model2 });

            return View();

            //	var vid = videoService.GetVideoById(quizI.Id);
            //			deg = Q.Alldegrees;
            //			sem = Q.Semester;
            //			if (quizI.Access)
            //			{
            //				Q.Access = true;
            //				quizI.Access = true;
            //				quizServes.Save();
            //			}
            //			else
            //			{
            //				Q.Access = false;
            //				quizI.Access = false;
            //				quizServes.Save();
            //			}
            //		}
            //		var quiz = new QuizTable(); // Temp 
            //		List<QuizTable> quizzes = new List<QuizTable>(); // Semester
            //            if (sem == Semester.الأول)
            //            {
            //                if (deg == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForOne1(); }
            //                if (deg == Alldegrees.الثاني) { quizzes = quizServes.GetAllquizesForTwo1(); }
            //if (deg == Alldegrees.الثالث) { quizzes = quizServes.GetAllquizesForThree1(); }
            //            }

            //			else
            //{
            //	if (deg == Alldegrees.الأول) { quizzes = quizServes.GetAllquizesForOne2(); }
            //	if (deg == Alldegrees.الثاني) { quizzes = quizServes.GetAllquizesForTwo2(); }
            //	if (deg == Alldegrees.الثالث) { quizzes = quizServes.GetAllquizesForThree2(); }
            //}
            //var q = quizServes.GetAllquizes();
            //return RedirectToAction("index", new { quiz = quiz, quizes = quizes });		


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FreeVideos()
        {
            var model = freeCoursrs.GetAll();
            var model2 = new FreeCourses();
            return View((model, model2));
        }

		[Authorize(Roles = (Constans.roleAdmin))]
        [HttpPost]
		public IActionResult UploadFreeVideo(FreeCourses AddVid)
        {
            freeCoursrs.SaveCours(AddVid);
            var model = freeCoursrs.GetAll();
            var model2 = new FreeCourses();
            return RedirectToAction("FreeVideos", (model, model2));
        }

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult DeleteVideo(List<FreeCourses> videos)
        {
            foreach (var v in videos)
            {
                if (v.Delete)
                {
                    var c = freeCoursrs.GetById(v.Id);
                    if (c != null)
                    {
                        freeCoursrs.remove(c);
                    }
                }
            }
            var model = freeCoursrs.GetAll();
            var model2 = new FreeCourses();
            return RedirectToAction("FreeVideos", (model, model2));
        }

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult DeleteCreditVideo(int id)
        {
            if(id == 0) { return View("NotFound404", "Home"); } // ملهاش لازمه؟
            var DeletedVideo=videoService.GetVideoById(id);
            if (DeletedVideo == null) { return View("NotFound404", "Home"); }
            videoService.Delete(id);


			AccessToVideo Model1 = new AccessToVideo();
			List<AccessToVideo> Model2 = new(); // ToList! 
			if (DeletedVideo.Semester == Semester.الأول)
			{

				if (DeletedVideo.Degree == Alldegrees.الأول) { Model2 = videoService.GetVideosForOne1(); }
				if (DeletedVideo.Degree == Alldegrees.الثاني) { Model2 = videoService.GetVideosForTwo1(); }
				if (DeletedVideo.Degree == Alldegrees.الثالث) { Model2 = videoService.GetVideosForThree1(); }
			}
			else
			{
				if (DeletedVideo.Degree == Alldegrees.الأول) { Model2 = videoService.GetVideosForOne2(); }
				if (DeletedVideo.Degree == Alldegrees.الثاني) { Model2 = videoService.GetVideosForTwo2(); }
				if (DeletedVideo.Degree == Alldegrees.الثالث) { Model2 = videoService.GetVideosForThree2(); }
			}
			return RedirectToAction("index", new { quiz = Model1, quizes = Model2 });

        }


	}
}
