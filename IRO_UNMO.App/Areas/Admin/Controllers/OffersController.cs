using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Areas.Admin.ViewModels;
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

namespace IRO_UNMO.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OffersController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        public OffersController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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
        public IActionResult Index()
        {
            OffersVM vm = new OffersVM();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b=>b.Country).OrderBy(a=>a.Start).ToList();
            return View(vm);
        }

        public IActionResult offer(int id = 0)
        {
            ViewBag.ID = id;
            EditOffersVM vm = new EditOffersVM();
        
            if (id == 0)
            {
                vm.Universities = _db.University.Include(a=>a.Country).Select(x => new SelectListItem()
                {
                    Value = x.UniversityId.ToString(),
                    Text = x.Name
                }).ToList();

                vm.Semesters = new List<SelectListItem>();

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
            
                vm.Programmes = new List<SelectListItem>();

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "other",
                    Text = "other"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "mechanic engineering",
                    Text = "mechanics engineering"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "civil engineering",
                    Text = "civil engineering"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "IT",
                    Text = "IT"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "business administration",
                    Text = "business administration"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "available in info link",
                    Text = "available in info link"
                });

                vm.Cycles = new List<SelectListItem>();

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "bachelor",
                    Text = "bachelor"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "winter",
                    Text = "winter"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "doctoral",
                    Text = "doctoral"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "other",
                    Text = "other"
                });
            }
            else
            {
                vm.OfferId = id;
                vm.Offer = _db.Offer.Where(a => a.OfferId == id).Include(x => x.University).ThenInclude(c=>c.Country).FirstOrDefault();
                vm.Universities = _db.University.Include(a=>a.Country).Select(x => new SelectListItem()
                {
                    Value = x.UniversityId.ToString(),
                    Text = x.Name
                }).ToList();

                vm.Semesters = new List<SelectListItem>();

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

                vm.Programmes = new List<SelectListItem>();

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "other",
                    Text = "other"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "mechanic engineering",
                    Text = "mechanics engineering"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "civil engineering",
                    Text = "civil engineering"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "IT",
                    Text = "IT"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "business administration",
                    Text = "business administration"
                });

                vm.Programmes.Add(new SelectListItem()
                {
                    Value = "available in info link",
                    Text = "available in info link"
                });

                vm.Cycles = new List<SelectListItem>();

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "bachelor",
                    Text = "bachelor"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "winter",
                    Text = "winter"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "doctoral",
                    Text = "doctoral"
                });

                vm.Cycles.Add(new SelectListItem()
                {
                    Value = "other",
                    Text = "other"
                });
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult offer(OffersVM vm)
        {
            Offer current = _db.Offer.Where(a => a.OfferId == vm.Offer.OfferId).Include(x => x.University).ThenInclude(c => c.Country).FirstOrDefault();
            if (current == null)
            {
                Offer novi = new Offer();
                novi.Semester = vm.Offer.Semester;
                novi.Programmes = vm.Offer.Programmes;
                novi.Cycles = vm.Offer.Cycles;
                novi.Start = vm.Offer.Start;
                novi.End = vm.Offer.End;
                novi.Info = vm.Offer.Info;
                novi.UniversityId = vm.Offer.UniversityId;
                _db.Offer.Add(novi);
                _db.SaveChanges();
            }
            else
            {
                current.Semester = vm.Offer.Semester;
                current.Programmes = vm.Offer.Programmes;
                current.Cycles = vm.Offer.Cycles;
                current.Start = vm.Offer.Start;
                current.End = vm.Offer.End;
                current.Info = vm.Offer.Info;
                current.UniversityId = vm.Offer.UniversityId;
                _db.SaveChanges();
            }
            return RedirectToAction("index", "offers", new { area = "admin" });
        }

        //[HttpDelete]
        public IActionResult delete(int id)
        {
            Offer current = _db.Offer.Where(a => a.OfferId == id).FirstOrDefault();
            if (current != null)
            {
                _db.Offer.Remove(current);
                _db.SaveChanges();
            }
            return RedirectToAction("index", "offers", new { area = "admin" });
        }
    }
}