using E_Recrutement.Common;
using E_Recrutement.Data;
using E_Recrutement.Models;
using E_Recrutement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Recrutement.Controllers
{
    [Route("candidate")]
    public class CandidateController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext _context;

        public CandidateController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (IsUsernameExists(model.Email))
            {
                ModelState.AddModelError("Email", "This account has already existed.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    FullName = model.FullName,
                    Age = model.Age,
                    Address = model.Address,
                    Email = model.Email,
                    CreateDate = DateTime.Now,
                    Phone = model.Phone,
                    Title = model.Title,
                    Status = null,
                    Slug = TextHelper.ToUnsignString(model.FullName).ToLower()

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Candidate");
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            if (TempData.ContainsKey("PasswordChanged") && (bool)TempData["PasswordChanged"])
            {
                ViewBag.PasswordChanged = true;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid account or password. Please try again !");
                    return View(model);
                }
            }
            return View(model);
        }

        [Route("logout")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [Route("change-password")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("change-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            await signInManager.RefreshSignInAsync(user);
            TempData["PasswordChanged"] = true;
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private bool IsUsernameExists(string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            return existingUser != null;
        }
    }
}
