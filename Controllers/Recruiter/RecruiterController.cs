using E_Recrutement.Data;
using Microsoft.EntityFrameworkCore;
using E_Recrutement.Models;
using E_Recrutement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_Recrutement.Common;

namespace E_Recrutement.Controllers.Recruiter
{
    //[Route("recruiter/auth")]
    [Route("recruiter")]
    public class RecruiterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;


        public RecruiterController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
            this.signInManager = signInManager;
        }


        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
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



        [Route("register/{id}")]
        public IActionResult Register()
        {
            ViewData["ProvinceId"] = new SelectList(_context.Provinces.OrderBy(p => p.Id), "Id", "Name");

            return View();
        }

        [Route("register/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Guid id, UpdateRecruiterViewModel model)
        {
            string POST_IMAGE_PATH = "images/employers/";

            User employer = _context.AppUsers.Where(u => u.Id == id).First();
            employer.FullName = model.FullName;
            var image = UploadImage.UploadImageFile(model.UrlAvatar, POST_IMAGE_PATH);
            employer.UrlAvatar = image;
            employer.Email = employer.UserName = model.Email;
            employer.NormalizedEmail = employer.NormalizedUserName = (employer.Email ?? model.Email).ToUpper();
            employer.CreateDate = DateTime.Now;
            employer.Description = model.Description;
            employer.Contact = model.Contact;
            employer.CompanySize = model.CompanySize;
            employer.Location = model.Location;
            employer.WebsiteURL = model.WebsiteURL;
            employer.ProvinceId = model.ProvinceId;
            employer.Phone = model.Phone;
            employer.Status = 1; // waiting to confirm
            _context.Update(employer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("register")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterToRecruiter()
        {
            ViewData["ProvinceId"] = new SelectList(_context.Provinces.OrderBy(p => p.Id), "Id", "Name");

            return View();
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterToRecruiter(RegisterRecruiterViewModel model)
        {
            string POST_IMAGE_PATH = "images/employers/";
            if (IsUsernameExists(model.Email))
            {
                ModelState.AddModelError("Email", "This account has already existed.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var image = UploadImage.UploadImageFile(model.UrlAvatar, POST_IMAGE_PATH);

                var employer = new User
                {
                    UserName = model.Email,
                    FullName = model.FullName,
                    Slug = TextHelper.ToUnsignString(model.FullName).ToLower(),
                    UrlAvatar = image,
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    NormalizedUserName = model.Email.ToUpper(),
                    CreateDate = DateTime.Now,
                    Description = model.Description,
                    Contact = model.Contact,
                    CompanySize = model.CompanySize,
                    Location = model.Location,
                    WebsiteURL = model.WebsiteURL,
                    ProvinceId = model.ProvinceId,
                    Title = "empty",
                    Phone = model.Phone,
                    Status = 2
                };
                var result = await userManager.CreateAsync(employer, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employer, "Recruiter");
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            ViewData["ProvinceId"] = new SelectList(_context.Provinces.OrderBy(p => p.Id), "Id", "Name");
            var employer = await _context.AppUsers.Where(e => e.Id == id).FirstAsync();
            return View(employer);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, UpdateRecruiterViewModel model)
        {
            string POST_IMAGE_PATH = "images/employers/";

            User employer = _context.AppUsers.Where(u => u.Id == id).First();

            //fix bug for null value
            if (model.FullName != null)
            {
                employer.FullName = model.FullName;
            }

            //fix bug for null value
            if (model.UrlAvatar != null)
            {
                string oldLogoImage = employer.UrlAvatar;
                var newLogoImage = UploadImage.UploadImageFile(model.UrlAvatar, POST_IMAGE_PATH);
                employer.UrlAvatar = newLogoImage;
                if (!string.IsNullOrEmpty(oldLogoImage))
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "employers", oldLogoImage);
                    DeleteImage.DeleteImageFile(oldImagePath);
                }
            }
            employer.Email = employer.UserName = model.Email;
            employer.NormalizedEmail = employer.NormalizedUserName = (employer.Email ?? model.Email).ToUpper();
            employer.Description = model.Description;
            employer.Contact = model.Contact;
            employer.CompanySize = model.CompanySize;
            employer.Location = model.Location;
            employer.WebsiteURL = model.WebsiteURL;
            employer.ProvinceId = model.ProvinceId;
            employer.Phone = model.Phone;
            _context.Update(employer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool IsUsernameExists(string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            return existingUser != null;
        }
    }
}
