using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
	public class AccountBLL : IAccountBLL
	{
		private readonly ApplicationContext db;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ICoursesNamesBLL coursesNamesBLL;

		public AccountBLL(ApplicationContext db, UserManager<ApplicationUser> userManager, ICoursesNamesBLL coursesNamesBLL)
		{
			this.db = db;
			this.userManager = userManager;
			this.coursesNamesBLL = coursesNamesBLL;
		}


		public IEnumerable<ApplicationUser> GetUsers()
		{

			return db.Users.ToList().OrderBy(x => x.UserName);

		}
		public IEnumerable<ApplicationUser> GetSchoolUsers()
		{
			return db.Users.Where(x => x.degree != Alldegrees.آخري &&x.degree != Alldegrees.أدمن)
				.ToList()
				.OrderBy(x => x.UserName);
		}
		public async void DeleteCreditCourseStd(int id)
		{
			var obj = coursesNamesBLL.GetById(id);
			var userid=obj.Id;

			if (obj != null)
			{
				coursesNamesBLL.Delete(obj);

			
				
			}
		}
	
	
		public IEnumerable<ApplicationUser> GetAdmins()
		{
			var all = GetUsers();
			var model=new List<ApplicationUser>();	
			foreach (var user in all)
			{
				if(user.degree==Alldegrees.أدمن )
				{
					 model.Add(user);
				}
			}
			return model;
		}
	
	
	}
}
