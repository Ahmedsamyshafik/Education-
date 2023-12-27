using Microsoft.AspNetCore.Identity;
using WEB.Models;

namespace WEB.ViewModel
{
	public static class DataSeeder
	{
		public static void Seed(IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			// Create an admin role if it doesn't exist
			var adminRole = roleManager.FindByNameAsync(Constans.roleAdmin).Result;
			if (adminRole == null)
			{
				adminRole = new IdentityRole("Admin");
				var result = roleManager.CreateAsync(adminRole).Result;
			}

			// Create an admin user if it doesn't exist
			var adminUser = userManager.FindByNameAsync("AhmedSamy").Result;
			if (adminUser == null)
			{
				adminUser = new ApplicationUser
				{
					UserName = "AhmedSamy",
					PhoneNumber="01095385375",
					IsInRole = true,
					degree=Alldegrees.أدمن
				};

				var result = userManager.CreateAsync(adminUser, "Alahly1907#").Result;

				// Add the admin user to the admin role
				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(adminUser, "Admin").Wait();
				}
			}
		}
	}
}
