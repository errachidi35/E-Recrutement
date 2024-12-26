using E_Recrutement.Common;
using E_Recrutement.Data;
using E_Recrutement.Models;
using E_Recrutement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace E_Recrutement.Controllers.Recruiter
{
    [Route("recruiter/offer")]
    [Authorize(Roles = "RECRUITER")]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("{id}")]
        public async Task<IActionResult> Index(Guid id, int? page)
        {
            int pageSize = 5; //number of jobs per page

            var jobs = await _context.Offers
                .Where(j => j.UserId == id)
                .Include(j => j.User)
                .Include(j => j.Province)
                .Include(j => j.ContractType)
                //.Include(j => j.Skills)
                .Include(j => j.Profil)
                .Include(j => j.Sector)
                .OrderByDescending(j => j.CreateDate)
                .ToListAsync();
            return View(jobs.ToPagedList(page ?? 1, pageSize));
        }

        [Route("{id}/create")]
        public IActionResult Create()
        {
            ViewData["ProvinceId"] = new SelectList(_context.Provinces.OrderBy(p => p.Id), "Id", "Name");
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes.OrderBy(t => t.Id), "Id", "Name");
            ViewData["ProfilId"] = new SelectList(_context.Profils.OrderBy(t => t.Name), "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.OrderBy(t => t.Name), "Id", "Name");
            //var skills = _context.Skills.OrderBy(s => s.Name).ToList();
            //ViewBag.SkillId = new MultiSelectList(skills, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("{id}/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, CreateOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                Offer job = new Offer()
                {
                    Name = model.Name,
                    Slug = TextHelper.ToUnsignString(model.Name).ToLower(),
                    CreateDate = DateTime.Now,
                    Description = model.Description,
                    //Introduce = model.Introduce,
                    //ObjectTarget = model.ObjectTarget,
                    //Experience = model.Experience,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    ProvinceId = model.ProvinceId,
                    ContractTypeId = model.ContractTypeId,
                    //Skills = _context.Skills.Where(s => model.SkillIds.Contains(s.Id)).ToList(),
                    ProfilId = model.ProfilId,
                    SectorId = model.SectorId,
                    UserId = id
                };
                _context.Offers.Add(job);
                await _context.SaveChangesAsync();
                return Redirect("/recruiter/offer/" + id);
            }
            return View(model);
        }

        [Route("{id}/update")]
        public IActionResult Update(int id)
        {
            ViewData["ProvinceId"] = new SelectList(_context.Provinces.OrderBy(p => p.Id), "Id", "Name");
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes.OrderBy(t => t.Id), "Id", "Name");
            ViewData["ProfilId"] = new SelectList(_context.Profils.OrderBy(t => t.Name), "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.OrderBy(t => t.Name), "Id", "Name");


            //var skills = _context.Skills.OrderBy(s => s.Name).ToList();
            //ViewBag.SkillId = new MultiSelectList(skills, "Id", "Name");

            var job = _context.Offers
                //.Include(j => j.Skills)
                .Where(j => j.Id == id)
                .First();

            var model = new UpdateOfferViewModel
            {
                Name = job.Name,
                Description = job.Description,
                //Introduce = job.Introduce,
                //ObjectTarget = job.ObjectTarget,
                //Experience = job.Experience,
                ProvinceId = job.ProvinceId,
                ContractTypeId = job.ContractTypeId,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                ProfilId = job.ProfilId,
                SectorId = job.SectorId,
                //SkillIds = job.Skills.Select(s => s.Id).ToList()
            };

            return View(model);
        }

        [Route("{id}/update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = _context.Offers
                    //.Include(j => j.Skills)
                    .FirstOrDefault(j => j.Id == id);

                if (job == null)
                {
                    return NotFound();
                }
                job.Name = model.Name;
                job.Slug = TextHelper.ToUnsignString(job.Name).ToLower();
                job.Description = model.Description;
                //job.Introduce = model.Introduce;
                //job.Experience = model.Experience;
                //job.ObjectTarget = model.ObjectTarget;
                job.MinSalary = model.MinSalary;
                job.MaxSalary = model.MaxSalary;
                job.ProvinceId = model.ProvinceId;
                job.ContractTypeId = model.ContractTypeId;
                job.ProfilId = model.ProfilId;
                job.SectorId = model.SectorId;

                //var selectedSkills = _context.Skills
                //    .Where(s => model.SkillIds.Contains(s.Id))
                //    .ToList();
                //job.Skills.Clear();
                //job.Skills.AddRange(selectedSkills);

                _context.Offers.Update(job);
                await _context.SaveChangesAsync();
                return Redirect("/recruiter/offer/" + job.UserId);
            }
            return View(model);
        }

        [HttpGet("{id}/delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                Offer job = _context.Offers/*.Include(j => j.Skills)*/.FirstOrDefault(s => s.Id == id);

                if (job == null)
                {
                    // Offer with the given id not found
                    return NotFound();
                }

                // Remove the associated OfferSkill records
                //foreach (var skill in job.skills.tolist())
                //{
                //    job.skills.remove(skill);
                //}

                // Now remove the job itself
                _context.Offers.Remove(job);

                _context.SaveChanges();
                return Redirect("/recruiter/offer/" + job.UserId);
            }
            catch (Exception)
            {
                // Handle any exceptions if necessary
                return RedirectToAction("Index"); // Redirect to some error page or handle the error as needed
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            var jobs = await _context.Offers
                /*.Include(j => j.Skills)*/ // Include the Skills related to the Offers
                .Where(j => listDelete.Contains(j.Id))
                .ToListAsync();

            foreach (var job in jobs)
            {
                // Remove the associated OfferSkill records
                //foreach (var skill in job.Skills.ToList())
                //{
                //    job.Skills.Remove(skill);
                //}

                _context.Offers.Remove(job);
            }

            await _context.SaveChangesAsync();

            var userId = jobs.FirstOrDefault()?.UserId;

            return RedirectToAction("Index", new { id = userId });
        }
    }
}
