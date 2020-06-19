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

        [HttpGet]
        public IActionResult details(string id)
        {
            ProfileVM vm = new ProfileVM
            {
                Applicant = _db.Applicant.Where(x => x.ApplicantId == id).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(b => b.University).FirstOrDefault(),
                Application = _db.Application.Where(a => a.ApplicantId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault(),
                Nominations = _db.Nomination.Where(a => a.ApplicantId == id).Include(b => b.University).ToList()
            };
            return View(vm);
        }
    }
}