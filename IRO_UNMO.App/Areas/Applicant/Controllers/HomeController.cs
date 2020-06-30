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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Applicant.Controllers
{
    //[Authorize(Roles="IncomingApplicant")]
    //[Authorize(Roles="OutgoingApplicant")]
    [Area("Applicant")]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        public HomeController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult details(string id)
        {
            ProfileVM vm = new ProfileVM
            {
                Applicant = _db.Applicant.Where(x => x.ApplicantId == id).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(b => b.University).ThenInclude(c=>c.Country).FirstOrDefault(),
                Application = _db.Application.Where(a => a.ApplicantId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault(),
                Nominations = _db.Nomination.Where(a => a.ApplicantId == id).Include(b => b.Offer).ThenInclude(c=>c.University).ThenInclude(q=>q.Country).ToList(),
                Timing = _db.Timing.FirstOrDefault()
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult create(CreateNewAppVM vm)
        {
            Application a = new Application();
            a.ApplicantId = vm.Applicant.ApplicantId;
            a.CreatedApp = DateTime.Now;
            a.LastEdited = DateTime.Now;
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

            return RedirectToAction("details", "home", new { id = applicant.ApplicantId });
        }
    }
}