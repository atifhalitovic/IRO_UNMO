using System;
using System.Text.Encodings.Web;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IRO_UNMO.App.Controllers
{
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
