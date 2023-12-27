using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.IRepo;
using WEB.Models;
using WEB.Repo;
using WEB.ViewModel;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add injection for Context!
builder.Services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(
  builder.Configuration.GetConnectionString("DefualtConnection1")
));

builder.Services.AddTransient<IAccountBLL, AccountBLL>();
builder.Services.AddTransient<IpdfBLL, pdfBLL>();
builder.Services.AddScoped<IQuizBLL, QuizBLL>();
builder.Services.AddScoped<IVideoBBL, VideoBBL>();
//builder.Services.AddScoped<ICreditCourses, CreditCourses>();
builder.Services.AddScoped<ICreditCourses, CreditCoursesBLL>();
builder.Services.AddScoped<ICoursesNamesBLL, CoursesNamesBLL>();
builder.Services.AddScoped<IFreeCoursrsBLL, FreeCoursrsBLL>();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<DataSeeder>();



// Add Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationContext>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // Add this line to enable session support

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = app.Services.CreateScope())
{
	var service = scope.ServiceProvider;
	DataSeeder.Seed(service);
}

app.Run();
