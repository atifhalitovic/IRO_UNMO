﻿using System;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ApplicantsController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        private readonly INotification _notificationService;

        public ApplicantsController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder, INotification notification)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _userManagementHelper = new UserManagementHelper(_db);
            _notificationService = notification;
        }

        public IActionResult Index()
        {
            ApplicantsVM vm = new ApplicantsVM();
            vm.IApplicants = _db.Applicant.Include(y => y.ApplicationUser).Include(q=>q.University).Where(a=>a.UniversityId!=2).ToList();
            vm.OApplicants = _db.Applicant.Include(a => a.ApplicationUser).Include(b => b.University).ToList().Where(x => x.UniversityId == 2).ToList();
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

        public IActionResult verified(string id)
        {
            Models.Applicant v = _db.Applicant.Include(a => a.ApplicationUser).Where(a=>a.ApplicantId==id).FirstOrDefault();
            string status = "";
            if (v.Verified == true)
            {
                v.Verified = false;
                status = "succesfully unverified!";
            }
            _db.Applicant.Update(v);
            _db.SaveChanges();
            _notificationService.sendToApplicant(id, id, new IRO_UNMO.App.Subscription.NotificationVM()
            {
                Message = "Your account verification has been changed. Now you are " + status,
                Url = "#"
            });
            return RedirectToAction("review", "applicants", new { id = v.ApplicantId });
        }

        public IActionResult unverified(string id)
        {
            Models.Applicant v = _db.Applicant.Include(a => a.ApplicationUser).Where(a => a.ApplicantId == id).FirstOrDefault();
            string status = "";
            if (v.Verified == false)
            {
                v.Verified = true;
                status = "succesfully verified!";
            }
            _db.Applicant.Update(v);
            _db.SaveChanges();
            _notificationService.sendToApplicant(id, id, new IRO_UNMO.App.Subscription.NotificationVM()
            {
                Message = "Your account verification has been changed. Now you are " + status,
                Url = "#"
            });
            return RedirectToAction("review", "applicants", new { id =  v.ApplicantId });
        }
    }
}