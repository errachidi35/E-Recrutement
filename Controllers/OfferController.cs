using E_Recrutement.Data;
using E_Recrutement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace E_Recrutement.Controllers
{
    [Route("offer")]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OfferController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("")]
        public IActionResult Index(string slug, int? page)
        {
            int pageSize = 5; //number of jobs per page
            var random = new Random();

            //random jobs - 6
            var jobList = _context.Offers.Include(j => j.Province).ToList();
            ViewBag.ListOffers = jobList.OrderBy(j => random.Next()).Take(6).ToList();


            //provinces - 4
            //ViewBag.ListProvinces = _context.Provinces.Include(p => p.Offers).Where(p => p.Offers.Count > 0).Take(4).ToList();

            //random blogs - 5
            //var blogList = _context.Blogs.Include(b => b.User).ToList();
            //ViewBag.ListBlogs = blogList.OrderBy(s => random.Next()).Take(5).ToList();

            var jobs = _context.Offers
                .OrderByDescending(j => j.Id)
                .Include(j => j.User)
                .Include(j => j.Profil)
                .Include(j => j.ContractType)
                .ToList();
            ViewBag.jobCount = _context.Offers.Count();

            if (slug != null)
            {
                var time = _context.ContractTypes.FirstOrDefault(t => t.Slug == slug);
                var province = _context.Provinces.FirstOrDefault(p => p.Slug == slug);
                var employer = _context.Users.FirstOrDefault(e => e.Slug == slug);

                if (time != null)
                {
                    jobs = (from t in _context.ContractTypes
                            join job in _context.Offers on t.Id equals job.ContractTypeId
                            orderby job.Id descending
                            where t.Slug == slug
                            select job).ToList();
                    ViewBag.ContractType = time;
                }
                else if (province != null)
                {
                    jobs = (from p in _context.Provinces
                            join job in _context.Offers on p.Id equals job.ProvinceId
                            orderby job.Id descending
                            where p.Slug == slug
                            select job).ToList();
                    ViewBag.Province = province;
                }
                else if (employer != null)
                {
                    jobs = (from e in _context.Users
                            join job in _context.Offers on e.Id equals job.UserId
                            orderby job.Id descending
                            where e.Slug == slug
                            select job).ToList();
                    ViewBag.Employer = employer;
                }
                //else
                //{
                //    var skill = _context.Skills.FirstOrDefault(s => s.Slug == slug);
                //    if (skill != null)
                //    {
                //        jobs = jobs.Where(j => j.(s => s.Slug == slug)).ToList();
                //        ViewBag.Skill = skill;
                //    }
                //}
                ViewBag.ContractTypeSlug = ViewBag.ContractType?.Slug;
                ViewBag.SkillSlug = ViewBag.Skill?.Slug;
                ViewBag.ProvinceSlug = ViewBag.Province?.Slug;
                ViewBag.EmployerSlug = ViewBag.Employer?.Slug;
             
                return View("~/Views/Job/Index.cshtml", jobs.ToPagedList(page ?? 1, pageSize));
            }
            return View("~/Views/Job/Index.cshtml", jobs.ToPagedList(page ?? 1, pageSize));
        }

        [Route("{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var random = new Random();

            //random jobs - 6
            var jobList = _context.Offers
                .Include(j => j.Province)
                .Include(j => j.User)
                .Include(j => j.ContractType)
                .Include(j => j.Profil)
                .ToList();
            ViewBag.ListOffers = jobList.OrderBy(j => random.Next()).Take(6).ToList();

            //random skills - 7
            var skillList = _context.Skills.Include(s => s.Offers).ToList();
            ViewBag.ListSkills = skillList.OrderBy(s => random.Next()).Where(s => s.Offers.Count > 0).Take(7).ToList();

            //provinces - 4
            ViewBag.ListProvinces = _context.Provinces.Include(p => p.Offers).Where(p => p.Offers.Count > 0).Take(4).ToList();

            //random blogs - 5
            //var blogList = _context.Blogs.Include(b => b.User).ToList();
            //ViewBag.ListBlogs = blogList.OrderBy(s => random.Next()).Take(5).ToList();

            var job = await _context.Offers
                .Where(j => j.Slug == slug)
                .Include(j => j.User)
                .Include(j => j.ContractType)
                .Include(j => j.Profil)
                //.Include(j => j.Skills)
                .FirstOrDefaultAsync();
            job.Popular++;
            await _context.SaveChangesAsync();

            //check existing CV
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id != null ? Guid.Parse(user.Id.ToString()) : Guid.Empty;
            var hasSubmittedCV = userId != Guid.Empty ? await HasSubmittedCV(userId, slug) : false;
            ViewBag.HasSubmittedCV = hasSubmittedCV;

            return View(job);
        }

        private async Task<bool> HasSubmittedCV(Guid userId, string jobSlug)
        {
            return await _context.CVs.AnyAsync(cv => cv.Offer.Slug == jobSlug && cv.UserId == userId);
        }

        [HttpGet]
        public IActionResult Search(string keywords, int? sectorId, int? contractTypeId, int? profileId, int? page)
        {
            ViewBag.Sectors = _context.Sectors.ToList();
            ViewBag.ContractTypes = _context.ContractTypes.ToList();
            ViewBag.Profiles = _context.Profils.ToList();

            var query = _context.Offers
                .Include(j => j.User)
                .Include(j => j.ContractType)
                .Include(j => j.Profil)
                .Include(j => j.Sector)
                .AsQueryable(); // Start with all jobs

            // Apply filtering based on the provided criteria

            // Filter by keywords
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(job => job.Name.Contains(keywords) || job.Description.Contains(keywords));
            }

            // Filter by sector
            if (sectorId.HasValue)
            {
                query = query.Where(job => job.SectorId == sectorId);
            }

            // Filter by contract type
            if (contractTypeId.HasValue)
            {
                query = query.Where(job => job.ContractTypeId == contractTypeId);
            }

            // Filter by profile
            if (profileId.HasValue)
            {
                query = query.Where(job => job.ProfilId == profileId);
            }

            // Execute the query to fetch filtered jobs
            var filteredJobs = query.OrderByDescending(j => j.Id).ToList();
            int pageSize = 5; // Number of jobs per page

            return View("~/Views/Job/Index.cshtml", filteredJobs.ToPagedList(page ?? 1, pageSize));
        }


        //[HttpGet]
        //public IActionResult Search(string keywords, int? sectorId, int? contractTypeId, int? profileId)
        //{
        //    ViewBag.Sectors = _context.Sectors.ToList();
        //    ViewBag.ContractTypes = _context.ContractTypes.ToList();
        //    ViewBag.Profiles = _context.Profils.ToList();

        //    var query = _context.Offers.AsQueryable(); // Start with all jobs

        //    // Apply filtering based on the provided criteria

        //    // Filter by keywords
        //    if (!string.IsNullOrWhiteSpace(keywords))
        //    {
        //        query = query.Where(job => job.Name.Contains(keywords) || job.Description.Contains(keywords));
        //    }

        //    // Filter by sector
        //    if (sectorId.HasValue)
        //    {
        //        query = query.Where(job => job.SectorId == sectorId);
        //    }

        //    // Filter by contract type
        //    if (contractTypeId.HasValue)
        //    {
        //        query = query.Where(job => job.ContractTypeId == contractTypeId);
        //    }

        //    // Filter by profile
        //    if (profileId.HasValue)
        //    {
        //        query = query.Where(job => job.ProfilId == profileId);
        //    }

        //    // Execute the query to fetch filtered jobs
        //    var filteredJobs = query.ToList();

        //    return View("~/Views/Job/Index.cshtml",filteredJobs);
        //}
    }
}
