using System;
using System.Linq;
using System.Text.Encodings.Web;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IRO_UNMO.App.Controllers
{
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult offers()
        {
            OffersVM vm = new OffersVM();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            return View(vm);
        }

        [ActionName("study-in-mostar")]
        public IActionResult mostar()
        {
            return View();
        }

        public IActionResult enrollment()
        {
            return View();
        }

        public IActionResult tuition()
        {
            return View();
        }

        public IActionResult activities()
        {
            return View();
        }

        public IActionResult administrative()
        {
            return View();
        }

        public IActionResult accomodation()
        {
            return View();
        }

        public IActionResult insurance()
        {
            return View();
        }

        public IActionResult emergency()
        {
            return View();
        }
    }
}
