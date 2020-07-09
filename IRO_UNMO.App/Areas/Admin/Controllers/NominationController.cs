using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NominationController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DashboardController> _logger;
        private readonly UrlEncoder _urlEncoder;

        public NominationController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder, ILogger<DashboardController> logger)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _logger = logger;
            _userManagementHelper = new UserManagementHelper(_db);
        }

        [Autorizacija(true, false, false)]
        public async Task<FileResult> download(string fileName)
        {
            var path = Path.Combine(
               Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }

        [Autorizacija(true, false, false)]
        public IActionResult docs(int id)
        {
            EditDocsNomVM model = new EditDocsNomVM();
            model.NominationId = id;
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.ApplicantId = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault().ApplicantId;
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.ApplicantId).FirstOrDefault();
            return View("docs", model);
        }

        [Autorizacija(true, false, false)]
        [HttpGet]
        public IActionResult view(int id)
        {
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).Include(q=>q.Offer).ThenInclude(a => a.University).ThenInclude(d=>d.Country).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(x => x.ApplicantId == model.Nomination.ApplicantId).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(c => c.University).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            model.Statuses = new List<SelectListItem>();
            model.Statuses.Add(new SelectListItem()
            {
                Value = "Unknown",
                Text = "Unknown"
            });

            model.Statuses.Add(new SelectListItem()
            {
                Value = "Review - on hold",
                Text = "Review - on hold"
            });

            model.Statuses.Add(new SelectListItem()
            {
                Value = "Review - successful",
                Text = "Review - succesful"
            });

            model.Statuses.Add(new SelectListItem()
            {
                Value = "Enrolled",
                Text = "Enrolled"
            });

            model.Statuses.Add(new SelectListItem()
            {
                Value = "Suspended",
                Text = "Suspended"
            });

            return View("view", model);
        }

        [Autorizacija(true, false, false)]
        public IActionResult comment(int id)
        {
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Nomination.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("comment", model);
        }

        [HttpPost]
        public IActionResult comment(ViewNomVM model)
        {
            var id1 = HttpContext.GetLoggedUser().Id;
            Nomination current = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).Include(a => a.University).FirstOrDefault();
           
            if(model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.AdministratorId = id1;
                newComment.IonId = current.NominationId;
                //newComment.Nomination = _db.Nomination.Where(a => a.NominationId == current.NominationId).FirstOrDefault();
                newComment.CommentTime = DateTime.Now;

                _db.Comment.Add(newComment);
                _db.SaveChanges();
            }
            else
            {
                return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
            }
            return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
        }

        [Autorizacija(true, false, false)]
        public IActionResult status(int id)
        {
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Nomination.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("status", model);
        }

        [HttpPost]
        public IActionResult status(ViewNomVM model)
        {
            Nomination current = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).Include(a => a.University).FirstOrDefault();
            current.StatusOfNomination = model.Nomination.StatusOfNomination;
            _db.SaveChanges();
            return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
        }
    }
}