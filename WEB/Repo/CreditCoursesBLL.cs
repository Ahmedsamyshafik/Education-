using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
	public class CreditCoursesBLL : ICreditCourses
	{
		private readonly ApplicationContext db;
		private readonly IWebHostEnvironment hostingEnvironment;

		public CreditCoursesBLL(ApplicationContext db, IWebHostEnvironment hostingEnvironment)
		{
			this.db = db;
			this.hostingEnvironment = hostingEnvironment;
		}

		public async Task<string> SaveCourseImge(CreditCourses course)
		{
			var ConsImgPass = "/assets/Images/Course";
			var ImagePath = $"{hostingEnvironment.WebRootPath}{ConsImgPass}";
			string uniqueFileName = Guid.NewGuid().ToString() + "_" + course.img.FileName;
			var uploadsFolder = Path.Combine(ConsImgPass, uniqueFileName);
			course.imgPath = uploadsFolder;
			var CurrentCourse = GetCourse(course.id);
			CurrentCourse.imgPath = uploadsFolder;
			uploadsFolder = Path.Combine(ImagePath, uniqueFileName);
			using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
			{
				await course.img.CopyToAsync(fileStream);
			}
			return "Done";
		}

		public void delete(CreditCourses course)
		{
			db.Remove(course);
			db.SaveChanges();
		}

		public async void Add(CreditCourses course)
		{
			// ModelState =>IMG
			var ConsImgPass = "/assets/Images/Course";
			var ImagePath = $"{hostingEnvironment.WebRootPath}{ConsImgPass}";
			string uniqueFileName = Guid.NewGuid().ToString() + "_" + course.img.FileName;
			var uploadsFolder = Path.Combine(ConsImgPass, uniqueFileName);
			course.imgPath = uploadsFolder;
			db.Add(course);
			Save();
			uploadsFolder = Path.Combine(ImagePath, uniqueFileName);
			using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
			{
				await course.img.CopyToAsync(fileStream);
			}
		}

		public List<CreditCourses> GetAllCourses()
		{
			return db.CeditCourses.OrderByDescending(x=>x.Date).ToList();
		}

		public CreditCourses GetCourse(int Id)
		{
			return db.CeditCourses.FirstOrDefault(x => x.id == Id);
		}
		public async Task<CreditCourses> GetCoursesAsync(int Id)
		{
			return await db.CeditCourses.FirstOrDefaultAsync(x => x.id == Id);

		}

		public void Save()
		{
			db.SaveChanges();
		}

		public async Task<string> SaveAsync()
		{
			await db.SaveChangesAsync();
			return "Done";

		}


        public async Task<CreditCourses> GetCourseAsync(int Id)
        {
            return await db.CeditCourses.FirstOrDefaultAsync(x => x.id == Id);
        }



		public async Task<string> ChangeNamePriceAsync(int id, string newName, decimal newPrice)
		{
			var x = await db.CeditCourses.FindAsync(id);
			x.name = newName;
			x.Price = newPrice;
			await db.SaveChangesAsync();
			return "ok";
		}

        public async Task<List<CreditCourses>> GetAllCoursesAsync()
        {
           return await db.CeditCourses.OrderByDescending(x => x.Date).ToListAsync();
        }
    }
}
