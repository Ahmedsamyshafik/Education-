using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WEB.IRepo;
using WEB.Models;
using WEB.Repo;
using WEB.ViewModel;

namespace WEB.Controllers
{
    [Authorize]

    public class AccountController : Controller
    {
        #region Inject
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IQuizBLL quizBLL;
        private readonly ICoursesNamesBLL coursesNamesBLL;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountBLL accountServes;
        public AccountController(
                                UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,
                                IAccountBLL AccountServes, RoleManager<IdentityRole> _roleManager
            , IQuizBLL quizBLL, ICoursesNamesBLL coursesNamesBLL
                                )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            accountServes = AccountServes;
            roleManager = _roleManager;
            this.quizBLL = quizBLL;
            this.coursesNamesBLL = coursesNamesBLL;
        }
        #endregion


        #region oneDevice 
        private string NormalizeUserAgent(string userAgent)
        {
            // Normalize the user agent string by removing unnecessary whitespace and formatting
            return userAgent.Replace(" ", "").ToLower();
        }
        private string GenerateDeviceIdentifier(string userAgent)
        {
            var normalizedUserAgent = NormalizeUserAgent(userAgent);
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(normalizedUserAgent));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginVM UserAccount = new LoginVM();
            return View(UserAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM UserAccount)
        {
            if (!User.Identity.IsAuthenticated)
            {

                if (ModelState.IsValid)
                {
                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                    var deviceIdentifier = GenerateDeviceIdentifier(userAgent);
                    ApplicationUser user = await userManager.FindByNameAsync(UserAccount.Name);
                    // For Admin Only!


                    if (user != null)
                    {
                        if (UserAccount.Degree == user.degree)
                        {
                            //Check IdDevice
                            // var DevId = user.DeviceId; // True
                            // if (DevId != null)
                            //{
                            //  if (DevId == deviceIdentifier)
                            //{
                            bool found = await userManager.CheckPasswordAsync(user, UserAccount.Password);
                            if (found)
                            {
                                await signInManager.SignInAsync(user, UserAccount.RememberMe);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid Password.");
                            }
                            // }
                            //else
                            //{
                            //   ModelState.AddModelError("", "Not allowed device..!");
                            //}

                        }
                        else
                        {
                            bool found = await userManager.CheckPasswordAsync(user, UserAccount.Password);
                            if (found)
                            {
                                //    user.DeviceId = deviceIdentifier;
                                //  await userManager.UpdateAsync(user);
                                //   await  signInManager.SignInAsync(user, log.RememberMe);
                                await signInManager.SignInAsync(user, true);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid Password.");
                            }
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("", "Student not in this age ");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid name");

                }
                return View(UserAccount);

            } // افصل الصفحتيييييييييييين ! 
            return RedirectToAction("index", "Home");

        }

    

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM NewUser)
    {
        if (!User.Identity.IsAuthenticated)
        {
            if (ModelState.IsValid)
            {
                // Create Account
                ApplicationUser user = new ApplicationUser();
                user.UserName = NewUser.Name;
                user.PasswordHash = NewUser.Password;
                user.degree = NewUser.Degree;
                user.PhoneNumber = NewUser.Phone;
                // Acutuly Creating...
                IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
                if (result.Succeeded)
                {
                    // create Cookie {SigninManger}
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(NewUser);
        }
        return RedirectToAction("index");
    }

    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        await signInManager.SignOutAsync();
        return RedirectToAction("Register", "Account");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> EditmyAcc()
    {
        string Name = User.Identity.Name;
        var user = await userManager.FindByNameAsync(Name);
        EditProfileVM model = new EditProfileVM()
        {
            Name = user.UserName,
            Phone = user.PhoneNumber
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditmyAcc(EditProfileVM model)
    {
        if (ModelState.IsValid)
        {
            // check pass

            string Name = User.Identity.Name;
            var user = await userManager.FindByNameAsync(Name);
            var result = await userManager.CheckPasswordAsync(user, model.OldPass);
            if (result)
            {
                user.PhoneNumber = model.Phone;
                await userManager.RemovePasswordAsync(user);
                await userManager.AddPasswordAsync(user, model.NewPass);
                await userManager.UpdateAsync(user);
            }
            ViewBag.Success = "Edit Done ";
        }
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = (Constans.roleAdmin))]
    public IActionResult AddStudent()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> AddStudent(RegisterVM NewUser)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = NewUser.Name;
            user.PasswordHash = NewUser.Password;
            user.IsInRole = true;
            user.degree = NewUser.Degree;
            user.PhoneNumber = NewUser.Phone;

            IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(NewUser);
    }

    [HttpGet]
    [Authorize(Roles = (Constans.roleAdmin))]
    public IActionResult AddAdmin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> AddAdmin(RegisterVM NewUser)
    {
        NewUser.Degree = Alldegrees.أدمن;
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = NewUser.Name;
            user.PasswordHash = NewUser.Password;
            user.IsInRole = true;
            user.degree = NewUser.Degree;
            // Acutuly Creating...
            IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
                ViewBag.success = "Added Done";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        return View(NewUser);
    }

    [Authorize(Roles = (Constans.roleAdmin))]
    [HttpGet]
    public IActionResult MyStudents()
    {
        IEnumerable<ApplicationUser> users = accountServes.GetSchoolUsers();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> MyStudents(IEnumerable<ApplicationUser> Editedusers)
    {
        if (ModelState.IsValid)
        {

            foreach (var u in Editedusers)
            {
                //To Acutauly Catch user from DB 

                var user = await userManager.FindByNameAsync(u.UserName);

                // Admin!! 
                //              if (user == null) { 
                //              if(u.UserName== "WaelKabil")
                //                  {
                //                      user= await userManager.FindByNameAsync("1");
                //}
                //              }

                #region Role
                user.IsInRole = u.IsInRole;
                var Level = u.degree.ToString();
                string R = string.Empty;
                if (Level == "الأول") R = Constans.One;
                if (Level == "الثاني") R = Constans.Two;
                if (Level == "الثالث") R = Constans.Three;
                if (Level == "آخري") R = Constans.Four;
                if (Level == "أدمن") R = Constans.roleAdmin;


                var isUserInRole = await userManager.IsInRoleAsync(user, R);
                // if user is checked?
                if (user.IsInRole)
                {
                    if (isUserInRole)
                    {// he is already have role
                        u.IsInRole = true; // Ui



                    }
                    else
                    {
                        // var roleExists = await roleManager.RoleExistsAsync("Admin");
                        var result = await userManager.AddToRoleAsync(user, R);
                        if (result.Succeeded)
                        {
                            u.IsInRole = true; // Ui
                            user.IsInRole = true; // DB
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                // Not Checked
                else
                {
                    // he was in Role ? 
                    if (isUserInRole)
                    {
                        var result = await userManager.RemoveFromRoleAsync(user, R);
                        if (result.Succeeded)
                        {
                            u.IsInRole = false; // Ui
                            user.IsInRole = false; // DB
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    // He wasn't in role and still not in role! 
                    else
                    {

                    }

                }

                #endregion
                #region Degree

                var Deg = u.degree;
                user.degree = Deg;
                await userManager.UpdateAsync(user);

                #endregion
            }
            return View(Editedusers);


        }





        return View();
    }

    [HttpGet]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> EditAsAdmin(string id) // For Course Student it CN id 
    {
        var user = await userManager.FindByIdAsync(id);
        return View(user);
    }

    [HttpGet]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> EditAsAdminForCourses(int id) // For Course Student it CN id 
    {
        var x = coursesNamesBLL.GetById(id);
        if (x == null) { return View("NotFound404", "Home"); }
        var user = await userManager.FindByIdAsync(x.UserId);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> EditAsAdminForCourses(ApplicationUser user) // User ! 
    {
        if (ModelState.IsValid)
        {

            var dbUser = await userManager.FindByNameAsync(user.UserName);

            if (dbUser == null)
            {
                // Handle the scenario where the user is not found
                return View("NotFound404", "Home");
            }
            dbUser.PhoneNumber = user.PhoneNumber;
            if (user.PasswordHash != null)
            {


                string pass = user.PasswordHash;
                await userManager.RemovePasswordAsync(dbUser);
                dbUser.PhoneNumber = user.PhoneNumber;
                await userManager.UpdateAsync(dbUser);
                //شغاله دي
                await userManager.AddPasswordAsync(dbUser, user.PasswordHash);
            }
            await userManager.UpdateAsync(dbUser);


            // Handle any exceptions that occur during the password update process
            ViewBag.ErrorMessage = "Updated Done ";
            return RedirectToAction("index", "Home");

        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> EditAsAdmin(ApplicationUser user) // User ! 
    {
        if (ModelState.IsValid)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);

            if (dbUser == null)
            {
                // Handle the scenario where the user is not found  
                return View("NotFound404", "Home");
            }
            dbUser.PhoneNumber = user.PhoneNumber;
            if (user.PasswordHash != null)
            {


                string pass = user.PasswordHash;
                await userManager.RemovePasswordAsync(dbUser);
                dbUser.PhoneNumber = user.PhoneNumber;
                await userManager.UpdateAsync(dbUser);
                //شغاله دي
                await userManager.AddPasswordAsync(dbUser, user.PasswordHash);
            }
            await userManager.UpdateAsync(dbUser);


            // Handle any exceptions that occur during the password update process
            ViewBag.ErrorMessage = "Updated Done ";
            return RedirectToAction("index", "Home");

        }
        return View(user);
    }

    [Authorize(Roles = (Constans.roleAdmin))]
    public async Task<IActionResult> Delete(string id)
    {
        var dbUser = await userManager.FindByIdAsync(id);
        if (dbUser == null)
        {
            return View("NotFound404", "Home");
        }
        var userRole = dbUser.degree;
        var result = await userManager.DeleteAsync(dbUser); // WOW ! 
        if (result.Succeeded)
        {
            // Delete From UsersQuiz
            var Delet = quizBLL.GetAllDataToUser(id);
            if (Delet != null)
            {
                quizBLL.DeleteRangeOdUserQuiz(Delet);

            }
            if (userRole == Alldegrees.أدمن)
            {
                return RedirectToAction("AdminsAccounts");
            }
            return RedirectToAction("MyStudents");
        }
        else
        {
            return View(dbUser);
        }
    }
    [Authorize(Roles = (Constans.roleAdmin))]
    [HttpGet]
    public async Task<IActionResult> EditStd(string? id)
    {
        var U = await userManager.FindByIdAsync(id);
        if (U == null)
        {
            return View("NotFound404", "Home");
        }
        return View(U);
    }
    [HttpPost]
    public async Task<IActionResult> EditStd(ApplicationUser user)
    {
        return View();
    }

    [Authorize(Roles = (Constans.roleAdmin))]
    [HttpGet]
    public IActionResult AdminsAccounts()
    {// Service for get only admins
        var model = accountServes.GetAdmins();
        return View(model);
    }

    [Authorize(Roles = (Constans.roleAdmin))]
    [HttpPost]
    public async Task<IActionResult> AdminsAccounts(List<ApplicationUser> Editedusers)
    {
        foreach (var u in Editedusers)
        {
            var user = await userManager.FindByNameAsync(u.UserName);
            if (user == null)
            {
                return View("NotFound404", "Home");
            }
            #region Role
            user.IsInRole = u.IsInRole;
            var Level = u.degree.ToString();
            string R = string.Empty;
            if (Level == "أدمن") R = Constans.roleAdmin;
            var isUserInRole = await userManager.IsInRoleAsync(user, R);
            if (user.IsInRole)
            {
                if (isUserInRole)
                {// he is already have role
                    u.IsInRole = true; // Ui
                }
                else
                {
                    // var roleExists = await roleManager.RoleExistsAsync("Admin");
                    var result = await userManager.AddToRoleAsync(user, R);
                    if (result.Succeeded)
                    {
                        u.IsInRole = true; // Ui
                        user.IsInRole = true; // DB
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            // Not Checked
            else
            {
                // he was in Role ? 
                if (isUserInRole)
                {
                    var result = await userManager.RemoveFromRoleAsync(user, R);
                    if (result.Succeeded)
                    {
                        u.IsInRole = false; // Ui
                        user.IsInRole = false; // DB
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                // He wasn't in role and still not in role! 
                else
                {
                }

            }

            #endregion
            #region Degree
            var Deg = u.degree;
            user.degree = Deg;
            await userManager.UpdateAsync(user);

            #endregion
        }
        return View(Editedusers);
    }



}
}
