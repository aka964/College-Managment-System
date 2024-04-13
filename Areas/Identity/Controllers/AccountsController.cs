using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementApp.MVC.Data;
using SchoolManagementApp.MVC.Models.ViewModels;
using SchoolManagementApp.MVC.Utilities;

namespace SchoolManagementApp.MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolManagementDbContext _context;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            SchoolManagementDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "User" });
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.RoleName == "Customer")
                    {
                        string[] nameParts = model.Name.Split(' ', 2);
                        string firstName = nameParts[0];
                        string lastName = nameParts.Length > 1 ? nameParts[1] : "";

                        var student = new Student
                        {
                            FirstName = firstName,
                            LastName = lastName,
                        };
                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();
                    }

                    if (!await _roleManager.RoleExistsAsync(model.RoleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                    }
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    if (!User.IsInRole(StaticData.role_admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["newAdminSignUp"] = user.UserName;
                    }
                    return RedirectToAction("Index", "Home", new { area = "User" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
    }

}
