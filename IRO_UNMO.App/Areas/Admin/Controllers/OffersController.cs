using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using IRO_UNMO.App.Areas.Admin.ViewModels;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        [Autorizacija(true, false, false)]
        [HttpGet]
        public IActionResult Index()
        {
            OffersVM vm = new OffersVM();
            vm.Offers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            vm.UOffers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start > DateTime.Now).OrderBy(y => y.Start).ToList();
            vm.EOffers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End <= DateTime.Now).OrderBy(y => y.Start).ToList();
            return View(vm);
        }

        [Autorizacija(true, false, false)]
        public IActionResult offer(int id = 0)
        {
            ViewBag.ID = id;

            EditOffersVM vm = _userManagementHelper.prepOffer();
            vm.OfferId = id;
            vm.Offer = _db.Offer.Where(a => a.OfferId == id).Include(x => x.University).ThenInclude(c => c.Country).FirstOrDefault();
            vm.Nominations = _db.Nomination.Where(a => a.OfferId == id).Include(b => b.Applicant).ThenInclude(c => c.ApplicationUser).ToList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult offer(EditOffersVM vm)
        {
            Offer current = _db.Offer.Where(a => a.OfferId == vm.Offer.OfferId).Include(x => x.University).ThenInclude(c => c.Country).FirstOrDefault();

            if (current == null)
            {
                Offer novi = new Offer();
                novi.Semester = vm.Offer.Semester;
                novi.Cycles = "";
                if (vm.Offer.LCycles != null)
                {
                    for (int i = 0; i < vm.Offer.LCycles.Count(); i++)
                    {
                        if (i == vm.Offer.LCycles.Count() - 1)
                        {
                            novi.Cycles += vm.Offer.LCycles[i];
                        }
                        else
                        {
                            novi.Cycles += vm.Offer.LCycles[i] + ", ";
                        }
                    }
                }
                novi.Programmes = "";
                if (vm.Offer.LProgrammes != null)
                {
                    for (int i = 0; i < vm.Offer.LProgrammes.Count(); i++)
                    {
                        if (i == vm.Offer.LProgrammes.Count() - 1)
                        {
                            novi.Programmes += vm.Offer.LProgrammes[i];
                        }
                        else
                        {
                            novi.Programmes += vm.Offer.LProgrammes[i] + ", ";
                        }
                    }
                }
                novi.Start = vm.Offer.Start.Date;
                novi.End = vm.Offer.End.Date;
                novi.Info = vm.Offer.Info;
                novi.UniversityId = vm.Offer.UniversityId;
                _db.Offer.Add(novi);
                _db.SaveChanges();
            }
            else
            {
                current.Semester = vm.Offer.Semester;
                if (vm.Offer.LCycles != null)
                {
                    current.Cycles = "";
                    for (int i = 0; i < vm.Offer.LCycles.Count(); i++)
                    {
                        if (i == vm.Offer.LCycles.Count() - 1)
                        {
                            current.Cycles += vm.Offer.LCycles[i];
                        }
                        else
                        {
                            current.Cycles += vm.Offer.LCycles[i] + ", ";
                        }
                    }
                }
                if (vm.Offer.LProgrammes != null)
                {
                    current.Programmes = "";
                    for (int i = 0; i < vm.Offer.LProgrammes.Count(); i++)
                    {
                        if (i == vm.Offer.LProgrammes.Count() - 1)
                        {
                            current.Programmes += vm.Offer.LProgrammes[i];
                        }
                        else
                        {
                            current.Programmes += vm.Offer.LProgrammes[i] + ", ";
                        }
                    }
                }
                //current.Start = vm.Offer.Start.Date;
                //current.End = vm.Offer.End.Date;
                current.Info = vm.Offer.Info;
                current.UniversityId = vm.Offer.UniversityId;
                _db.SaveChanges();
            }
            return RedirectToAction("index", "offers", new { area = "admin" });
        }

        [Autorizacija(true, false, false)]
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