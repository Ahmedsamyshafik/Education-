using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB.Models;
using WEB.ViewModel;

namespace WEB.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<QuizTable> QuizsTable { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UsersQuiz> UsersQuiz { get; set; }
        public DbSet<AccessToVideo> AccessToVideo { get; set; }
        public DbSet<PdfMaterial> PdfMaterial { get; set; }
        public DbSet<CreditCourses> CeditCourses { get; set; }
        public DbSet<CoursesNames> coursesNames { get; set; }
        public DbSet<FreeCourses> freeCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                 new IdentityRole { Id = "1", Name = "One", NormalizedName = "ONE" },
                 new IdentityRole { Id = "2", Name = "Two", NormalizedName = "TWO" },
                 new IdentityRole { Id = "3", Name = "Three", NormalizedName = "THREE" },
                 new IdentityRole { Id = "4", Name = "Four", NormalizedName = "FOUR" },
                 new IdentityRole { Id = "5", Name = "Admin", NormalizedName = "ADMIN" }
                  );

		
		
			//Seed Data

			//public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
			//{
			//	// Other middleware configuration

			//	// Seed the admin user and role
			//	SeedData.Initialize(serviceProvider);
			//}

			//         var passwordHasher = new PasswordHasher<ApplicationUser>().HashPassword(null,"f"); 
			//         //   var passwordHash = passwordHasher.HashPassword(, "WaelKabilAlahly1907#");
			//         var passwordHash = "WaelKabilAlahly1907#";


			//builder.Entity<ApplicationUser>().HasData(
			//             new ApplicationUser
			//             {
			//                 Id = "1",
			//                 UserName = "WaelKabil",
			//                 degree = Alldegrees.أدمن
			//             ,
			//                 PhoneNumber = "0",
			//                 IsInRole = true,
			//                 PasswordHash = passwordHash
			//             }
			//             );

			//         builder.Entity<IdentityUserRole<string>>().HasData(
			//     new IdentityUserRole<string> { RoleId = "5", UserId = "1" }
			// );


		}
    }
}
