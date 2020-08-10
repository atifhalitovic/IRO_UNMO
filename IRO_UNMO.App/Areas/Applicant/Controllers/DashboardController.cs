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
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class DashboardController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        public DashboardController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _userManagementHelper = new UserManagementHelper(_db);
        }

        [Autorizacija(false, true, true)]
        public IActionResult Index()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        [Autorizacija(false, true, true)]
        [HttpGet]
        [ActionName("profile")]
        public IActionResult details(string id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            ProfileVM vm = new ProfileVM
            {
                Applicant = _db.Applicant.Where(x => x.ApplicantId == id).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(b => b.University).ThenInclude(c=>c.Country).FirstOrDefault(),
                Application = _db.Application.Where(a => a.ApplicantId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault(),
                Nominations = _db.Nomination.Where(a => a.ApplicantId == id).Include(b => b.Offer).ThenInclude(c=>c.University).ThenInclude(q=>q.Country).ToList(),
                Timing = _db.Timing.FirstOrDefault()
            };
            return View(vm);
        }

        [Autorizacija(false, true, false)]
        [HttpPost]
        public IActionResult create(CreateNewAppVM vm)
        {
            Application a = new Application();
            a.ApplicantId = vm.Applicant.ApplicantId;
            a.CreatedApp = DateTime.Now;
            a.LastEdited = DateTime.Now;
            a.Finished = false;
            a.StatusOfApplication = "Unknown";

            Models.Applicant applicant = _db.Applicant.Where(xa => xa.ApplicantId == a.ApplicantId).Include(xq => xq.ApplicationUser).ThenInclude(xe => xe.Country).Include(xw => xw.University).FirstOrDefault();

            Info newInfo = new Info();
            newInfo.CitizenshipId = applicant.ApplicationUser.CountryId;
            newInfo.Citizenship = _db.Country.Where(t => t.CountryId == newInfo.CitizenshipId).FirstOrDefault();
            a.Infos = newInfo;
            _db.Info.Add(newInfo);

            Contact newContact = new Contact();
            newContact.Email = applicant.ApplicationUser.Email;
            newContact.Telephone = applicant.ApplicationUser.PhoneNumber;
            newContact.Country = null;
            newContact.CountryId = null;
            a.Contacts = newContact;
            _db.Contact.Add(newContact);

            Language newLang = new Language();
            a.Languages = newLang;
            _db.Language.Add(newLang);

            HomeInstitution newHI = new HomeInstitution();
            newHI.OfficialName = applicant.University.Name;
            newHI.LevelOfEducation = applicant.StudyCycle;
            newHI.StudyProgramme = applicant.StudyField;
            a.HomeInstitutions = newHI;
            _db.HomeInstitution.Add(newHI);

            Other newOther = new Other();
            a.Others = newOther;
            _db.Other.Add(newOther);

            Documents newDocs = new Documents();
            a.Documents = newDocs;
            _db.Documents.Add(newDocs);

            _db.Application.Add(a);
            _db.SaveChanges();

            return RedirectToAction("profile", "dashboard", new { id = applicant.ApplicantId });
        }

        [Autorizacija(false, false, true)]
        [HttpGet]
        public IActionResult offers()
        {
            OffersVM vm = new OffersVM();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            vm.UOffers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start > DateTime.Now).OrderBy(y => y.Start).ToList();
            vm.EOffers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End <= DateTime.Now).OrderBy(y => y.Start).ToList();
            return View(vm);
        }

        [ActionName("study-in-mostar")]
        public IActionResult mostar()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult enrollment()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult tuition()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult activities()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult administrative()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult accomodation()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult insurance()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }

        public IActionResult emergency()
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            return View();
        }
    }
}