﻿using System;
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
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        public NewsController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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
            NewsVM vm = new NewsVM();
            vm.Comments = _db.Comment.Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).Where(y => y.AdministratorId == null && y.Seen==false).OrderByDescending(x=>x.CommentTime).ToList();
            return View(vm);
        }

        public IActionResult seen(int id)
        {
            var n = _db.Comment.Where(x => x.CommentId == id).FirstOrDefault();
            if (n != null)
            {
                n.Seen = true;
                _db.SaveChanges();
            }
            return RedirectToAction("index", "news", new { area = "admin" });
        }

        [Autorizacija(true, false, false)]
        public IActionResult partner(int id = 0)
        {
            ViewBag.ID = id;
            EditPartnersVMa vm = new EditPartnersVMa();
            if (id == 0)
            {
                vm.Countries = _db.Country.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = x.Name
                }).OrderBy(a => a.Text).ToList();
            }
            else
            {
                vm.UniversityId = id;
                vm.University = _db.University.Where(a => a.UniversityId == id).Include(x => x.Country).FirstOrDefault();
                vm.Countries = _db.Country.Select(x => new SelectListItem()
                {
                    Value = x.CountryId.ToString(),
                    Text = x.Name
                }).ToList();
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult partner(PartnersVM vm)
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

            return RedirectToAction("index", "partners", new { area = "admin" });
        }

        [Autorizacija(true, false, false)]
        [HttpDelete]
        public IActionResult delete(int id)
        {
            University current = _db.University.Where(a => a.UniversityId == id).FirstOrDefault();
            if (current == null)
            {
                _db.University.Remove(current);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}