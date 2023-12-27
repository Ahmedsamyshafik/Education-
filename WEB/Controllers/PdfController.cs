using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Controllers
{
    [Authorize]
    public class PdfController : Controller
    {
        #region Inject and Ctor
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IpdfBLL pdfServes;
        private readonly UserManager<ApplicationUser> userManager;

        public PdfController(IWebHostEnvironment hostEnvironment,
            IpdfBLL pdfServes,
             UserManager<ApplicationUser> _userManager)
        {
            _hostEnvironment = hostEnvironment;
            this.pdfServes = pdfServes;
            userManager = _userManager;
        }
        #endregion
        public async  Task<IActionResult> Index(int id)
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
			PdfMaterial pdf = new();

            List<PdfMaterial> pdfs = new(); // ToList! 
            if (id == 1) { pdfs = pdfServes.GetAllPdfForOne1(); }
            if (id == 2) { pdfs = pdfServes.GetAllPdfForTwo1(); }
            if (id == 3) { pdfs = pdfServes.GetAllPdfForThree1(); }

            ViewBag.id = id;
            return View((pdf, pdfs));
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
			if (user == null) 
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
			PdfMaterial pdf = new();

			List<PdfMaterial> pdfs = new(); // ToList! 
			if (id == 1) { pdfs = pdfServes.GetAllPdfForOne2(); }
			if (id == 2) { pdfs = pdfServes.GetAllPdfForTwo2(); }
			if (id == 3) { pdfs = pdfServes.GetAllPdfForThree1(); }

			ViewBag.id = id;
			return View((pdf, pdfs));
		}

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult AddPdf(PdfMaterial pdf)
        {
            if (pdf.File != null && pdf.File.Length > 0)
            {
                pdfServes.SavingPdf(pdf);
            }
            var Deg = pdf.alldegrees;
            var Sem = pdf.Semester;
            int ind = 0;
            if (Deg == Alldegrees.الأول) { ind = 1; }
            if (Deg == Alldegrees.الثاني) { ind = 2; }
            if (Deg == Alldegrees.الثالث) { ind = 3; }

            if (Sem == Semester.الأول)
            {
                return RedirectToAction("index", new { id = ind });
            }
            if(Sem == Semester.الثاني)
            {
                return RedirectToAction("index2", new { id = ind });
            }

            else
            {
                return RedirectToAction("index", new { id = 1 });
            }
        }

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult DeletePdf(int id)
        {
            if(id == 0)
            {
				return View("NotFound404", "Home");
			}
            var Deg = (pdfServes.GetPdfById(id)).alldegrees;

            pdfServes.DeletePdf(id);


            PdfMaterial pdf = new();

            List<PdfMaterial> pdfs = new(); // ToList! 
            if (Deg==Alldegrees.الأول) { pdfs = pdfServes.GetAllPdfForOne1(); }
            if (Deg==Alldegrees.الثاني) { pdfs = pdfServes.GetAllPdfForTwo1(); }
            if (Deg==Alldegrees.الثالث) { pdfs = pdfServes.GetAllPdfForThree1(); }
            ViewBag.id = id;

            return View("index", (pdf, pdfs));
        }

		[Authorize(Roles = (Constans.roleAdmin))]
		public IActionResult pdfAccess(List<PdfMaterial> pdfs)
        {
            var deg = Alldegrees.الأول;
            var sem = Semester.الثاني;
            foreach (var pdf in pdfs)
            {

                var Q = pdfServes.GetPdfById(pdf.Id);
                deg = Q.alldegrees;
                sem = Q.Semester;
                if (pdf.Access)
                {
                    Q.Access = true;
                    pdf.Access = true;
                    pdfServes.Save();
                }
                else
                {
                    Q.Access = false;
                    pdf.Access = false;
                    pdfServes.Save();
                }
            }
            PdfMaterial model1 = new();
            int id = 0;
            List<PdfMaterial> model2 = new(); // ToList! 
            if (deg == Alldegrees.الأول) {id = 1;pdfs = pdfServes.GetAllPdfForOne1(); }
            if (deg == Alldegrees.الثاني) { id = 2; pdfs = pdfServes.GetAllPdfForTwo1(); }
            if (deg == Alldegrees.الثالث) { id = 3; pdfs = pdfServes.GetAllPdfForThree1(); }

            ViewBag.id = id;
            return View("index", (model1, model2));

        }


        public async Task<IActionResult> DisplayPDf(int id) {
           
            string name = User.Identity.Name;
            if(string.IsNullOrEmpty(name) || id == 0) {return View("NotFound404", "Home");}
            var user=await userManager.FindByNameAsync(name);
            if (user == null) { return View("NotFound404", "Home"); }

           var pdf= pdfServes.GetPdfById(id);
            if (pdf == null) { return View("NotFound404", "Home"); }
            var result = await userManager.IsInRoleAsync(user, Constans.roleAdmin);
            if (pdf.Access || result )//Admin User 
            {// Code To Display
                return View(pdf);
            }
			PdfMaterial model1 = new();

			List<PdfMaterial> pdfs = new(); // ToList! 
			if (pdf.alldegrees == Alldegrees.الأول) { pdfs = pdfServes.GetAllPdfForOne1(); }
			if (pdf.alldegrees == Alldegrees.الثاني) { pdfs = pdfServes.GetAllPdfForTwo1(); }
			if (pdf.alldegrees == Alldegrees.الثالث) { pdfs = pdfServes.GetAllPdfForThree1(); }
			ViewBag.id = id;

			return View("index", (model1, pdfs));
        }
    }
}
