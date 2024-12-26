using E_Recrutement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;


namespace E_Recrutement.Controllers.Recruiter
{
    [Route("employer")]
    public class HomeRecruiterController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HomeRecruiterController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("index/{id}")]
        [Route("{id}")]
        public IActionResult Index(Guid id, int? page)
        {
            int pageSize = 5; //number of item per page

            //job
            ViewBag.jobCount = _context.Offers.Where(cv => cv.UserId == id).Count();
            var jobs = _context.Offers
                .Where(j => j.UserId == id)
                .Include(j => j.User)
                //.Include(j => j.Name)
                //.Include(j => j.Province)
                .ToList();

            //cv
            ViewBag.cvCount = _context.CVs
                .Where(cv => cv.Offer.UserId == id)
                .Include(cv => cv.Offer)
                .Include(cv => cv.User)
                .Count();
            ViewBag.cvList = _context.CVs
                .Where(cv => cv.Offer.UserId == id)
                .OrderByDescending(cv => cv.ApplyDate)
                .Include(cv => cv.User)
                .Include(cv => cv.Offer)
                .Take(5)
                .ToList();



            return View(jobs.ToPagedList(page ?? 1, pageSize));
        }
    }
}
