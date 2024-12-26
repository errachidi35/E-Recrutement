using E_Recrutement.Data;
using E_Recrutement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_Recrutement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


       
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
                _context = context;
                _logger = logger;
        }

        public IActionResult Index()

        {
            DateTime oneWeekAgo = DateTime.Today.AddDays(-7);

            // Count total number of jobs posted within the last week
            int totalJobCount = _context.Offers.Where(j => j.CreateDate >= oneWeekAgo).Count();
            ViewBag.jobCount = totalJobCount;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
