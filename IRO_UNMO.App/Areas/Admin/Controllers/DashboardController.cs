using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.Subscription;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        [Autorizacija(true, false, false)]
        public IActionResult Index()
        {
            return View();
        }

        [Autorizacija(true, false, false)]
        [HttpGet]
        public IActionResult timing()
        {
            TimingVM vm = new TimingVM();
            vm.Semesters = new List<SelectListItem>();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            vm.Semesters.Add(new SelectListItem()
            {
                Value = "summer",
                Text = "summer"
            });

            vm.Semesters.Add(new SelectListItem()
            {
                Value = "winter",
                Text = "winter"
            });
            vm.current = _db.Timing.FirstOrDefault();
            return View(vm);
        }

        [HttpPost]
        public IActionResult timing(TimingVM vm)
        {
            Timing current = _db.Timing.FirstOrDefault();
            vm.Semesters = new List<SelectListItem>();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            vm.Semesters.Add(new SelectListItem()
            {
                Value = "summer",
                Text = "summer"
            });

            vm.Semesters.Add(new SelectListItem()
            {
                Value = "winter",
                Text = "winter"
            });
            current.Semester = vm.current.Semester;
            current.From = vm.current.From;
            current.To = vm.current.To;
            _db.SaveChanges();

            return RedirectToAction("timing", "dashboard", new { area = "admin" });
        }
    }
}