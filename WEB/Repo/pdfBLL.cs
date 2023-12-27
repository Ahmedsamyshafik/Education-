using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
    public class pdfBLL :IpdfBLL
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ICreditCourses oS;
        private readonly string PdfPath;
        private readonly string wwwrootpath = "/assets/Images/PDF";


        public pdfBLL(ApplicationContext db, IWebHostEnvironment hostingEnvironment, ICreditCourses OS)
        {
            this.db = db;
            this.hostingEnvironment = hostingEnvironment;
            oS = OS;
            PdfPath = $"{hostingEnvironment.WebRootPath}{wwwrootpath}";

        }

        public List<PdfMaterial> GetAllPDFs1()
        {
            return db.PdfMaterial.Where(x => x.Semester == Semester.الأول).ToList();
        }

        public List<PdfMaterial> GetAllPDFs2()
        {
            return db.PdfMaterial.Where(x => x.Semester == Semester.الثاني).ToList();
        }

        public List<PdfMaterial> GetAllPdfForOne1()
        {
            var Pdfs = GetAllPDFs1();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الأول" && pdf.Semester.ToString() == "الأول")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
            return ActulyPdfs.OrderByDescending(x=>x.Date).ToList();
        }
        public List<PdfMaterial> GetAllPdfForTwo1()
        {
            var Pdfs = GetAllPDFs1();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الثاني" && pdf.Semester.ToString() == "الأول")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}
		public List<PdfMaterial> GetAllPdfForThree1()
        {
            var Pdfs = GetAllPDFs1();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الثالث" )
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}
		public List<PdfMaterial> GetAllPdfForFour1()
        {
            var Pdfs = GetAllPDFs1();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "آخري" && pdf.Semester.ToString() == "الأول")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}

		public List<PdfMaterial> GetAllPdfForOne2()
        {
            var Pdfs = GetAllPDFs2();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الأول" && pdf.Semester.ToString() == "الثاني")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}
		public List<PdfMaterial> GetAllPdfForTwo2()
        {
            var Pdfs = GetAllPDFs2();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الثاني" && pdf.Semester.ToString() == "الثاني")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}
		public List<PdfMaterial> GetAllPdfForThree2()
        {
            var Pdfs = GetAllPDFs2();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "الثالث" && pdf.Semester.ToString() == "الثاني")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}
		public List<PdfMaterial> GetAllPdfForFour2()
        {
            var Pdfs = GetAllPDFs2();
            List<PdfMaterial> ActulyPdfs = new List<PdfMaterial>(); // Initialize the list
            foreach (var pdf in Pdfs)
            {
                if (pdf.alldegrees.ToString() == "آخري" && pdf.Semester.ToString() == "الثاني")
                {
                    ActulyPdfs.Add(pdf);
                }
            }
			return ActulyPdfs.OrderByDescending(x => x.Date).ToList();
		}

		public PdfMaterial GetPdfById(int id)
        {
            return db.PdfMaterial.FirstOrDefault(p => p.Id == id);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void SaveAsync()
        {
            db.SaveChangesAsync();
        }

        public async void SavingPdf(PdfMaterial pdfMaterial)
        {
            var ConsPdfPass = "/assets/Images/PDF";
            var PdfPath = $"{hostingEnvironment.WebRootPath}{ConsPdfPass}";
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + pdfMaterial.File.FileName;
            var uploadsFolder = Path.Combine(ConsPdfPass, uniqueFileName);
            pdfMaterial.Pdf_Path = uploadsFolder;

            db.PdfMaterial.Add(pdfMaterial);
            Save();

            uploadsFolder = Path.Combine(PdfPath, uniqueFileName);
            using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
            {
                await pdfMaterial.File.CopyToAsync(fileStream);
            }
        }

        public async void SavePdfWithCourses(PdfMaterial pdf, int Co_Id)
        {
            var ConsPdfPass = "/assets/Images/PDF";
            var PdfPath = $"{hostingEnvironment.WebRootPath}{ConsPdfPass}";
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + pdf.File.FileName;
            var uploadsFolder = Path.Combine(ConsPdfPass, uniqueFileName);
            pdf.Pdf_Path = uploadsFolder;
            db.Add(pdf);
            db.SaveChanges();

            var Course = oS.GetCourse(Co_Id);

            CoursesNames coursesNames = new()
            {
                CourseId = Co_Id,
                Name = Course.name,
                PdfId = pdf.Id,
            };
            db.Add(coursesNames);
            db.SaveChanges();

            uploadsFolder = Path.Combine(PdfPath, uniqueFileName);
            using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
            {
                await pdf.File.CopyToAsync(fileStream);
            }
        }

        public void DeletePdf(int id) {
            var pdf = GetPdfById(id);
            if(pdf != null)
            {
                db.Remove(pdf);
                db.SaveChanges();

            }
        }
    }
}
