using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ApplicantController> _logger;
        private readonly UrlEncoder _urlEncoder;

        public AdminController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder, ILogger<ApplicantController> logger)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _logger = logger;
            _userManagementHelper = new UserManagementHelper(_db);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult partner(int id=0)
        {
            EditPartnersVM vm = new EditPartnersVM();
            if(id==0)
            {
                vm.Countries = _db.Country.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = x.Name
                }).OrderBy(a=>a.Text).ToList();
            }
            else
            {
                vm.UniversityId = id;
                vm.University = _db.University.Where(a => a.UniversityId == id).Include(x => x.Country).FirstOrDefault();
                vm.Countries = _db.Country.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = ""//x.Name
                }).ToList();
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult partner(EditPartnersVM vm)
        {
            University current = _db.University.Where(a => a.UniversityId == vm.UniversityId).FirstOrDefault();
            if (current == null)
            {
                University novi = new University();
                novi.Name = vm.University.Name;
                novi.Code = vm.University.Code;
                novi.CountryId = vm.University.CountryId;
                _db.University.Add(novi);

                _db.SaveChanges();
            }
            else
            {
                current.Name = vm.University.Name;
                current.Code = vm.University.Code;
                current.CountryId = vm.University.CountryId;
                _db.SaveChanges();
            }

            return RedirectToAction("partners", "admin");
        }

        [HttpGet]
        public IActionResult partners()
        {
            PartnersVM vm = new PartnersVM();
            vm.Universities = _db.University.Include(a=>a.Country).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult applicants()
        {
            ApplicantsVM vm = new ApplicantsVM();
            vm.Applications = _db.Application.Include(a => a.Applicant).ThenInclude(y => y.ApplicationUser).Include(a => a.Applicant).ThenInclude(b => b.University).ToList();
            vm.OApplicants = _db.Applicant.Include(a => a.ApplicationUser).Include(b => b.University).ToList().Where(x=>x.UniversityId==23).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult applications()
        {
            ApplicationsVM vm = new ApplicationsVM();
            vm.Applications = _db.Application.Include(a=>a.Applicant).ThenInclude(b=>b.ApplicationUser).ToList();
            vm.Nominations = _db.Nomination.Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult review(string id)
        {
            ProfileVM vm = new ProfileVM
            {
                Applicant = _db.Applicant.Where(x => x.ApplicantId == id).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(b => b.University).FirstOrDefault(),
                Application = _db.Application.Where(a => a.ApplicantId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault(),
                Nominations = _db.Nomination.Where(a => a.ApplicantId == id).Include(b => b.University).ToList()
            };
            return View(vm);
        }

        public IActionResult Login()
        {
            LoginVM model = new LoginVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.UniqueCode == model.UniqueCode);
            if (user == null) return RedirectToAction("AccessDenied");

            var userRole = _userManager.GetRolesAsync(user).Result.Single();

            if (userRole == "Administrator")
                return RedirectToAction("index", "admin", new { @id = user.Id });

            else if (userRole == "IncomingApplicant" || userRole == "OutgoingApplicant")
                return RedirectToAction("details", "applicant", new { @id = user.Id });

            return View(model);
        }
    }
}