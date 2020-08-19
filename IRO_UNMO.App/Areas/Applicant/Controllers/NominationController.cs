using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.Subscription;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IRO_UNMO.App.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class NominationController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly INotification _notificationService;

        public NominationController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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

        public IActionResult delete(string fileType, string fileName, int id)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\uploads\\", fileName);

            try
            {
                System.IO.File.Delete(path);
            }
            catch
            {

            }

            var docs = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();

            if (fileType == "LearningAgreement")
            {
                docs.LearningAgreement = null;
            }
            if (fileType == "CV")
            {
                docs.CV = null;
            }
            if (fileType == "Passport")
            {
                docs.Passport = null;
            }
            if (fileType == "EngProficiency")
            {
                docs.EngProficiency = null;
            }
            if (fileType == "TranscriptOfRecords")
            {
                docs.TranscriptOfRecords = null;
            }
            if (fileType == "MotivationLetter")
            {
                docs.MotivationLetter = null;
            }
            if (fileType == "ReferenceLetter")
            {
                docs.ReferenceLetter = null;
            }
            _db.SaveChanges();

            return RedirectToAction("view", "nomination", new { id });
        }

        [Autorizacija(false, false, true)]
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

        [Autorizacija(false, false, true)]
        public IActionResult create(string id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            CreateNewNomVM model = new CreateNewNomVM();
            model.ApplicantId = id;
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == id).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();

            List<Offer> allOffers = _db.Offer.Include(a => a.University).ThenInclude(b => b.Country).Where(x => x.Start <= DateTime.Now && x.End >= DateTime.Now).OrderBy(a => a.Start).ToList();
            List<Offer> userNominations = _db.Nomination.Where(a => a.ApplicantId == id).Select(q=>q.Offer).OrderBy(a => a.Start).ToList();

            List<Offer> offersWithoutNom = allOffers.Except(userNominations).ToList();

            if (userNominations.Count() != 0 && userNominations.Count() <= allOffers.Count()) 
            {
                model.Offers = offersWithoutNom;
            }
            else
            {
                model.Offers = allOffers;
            }
            return View("create", model);
        }

        [HttpPost]
        public IActionResult create(CreateNewNomVM vm, int offerId)
        {
            Nomination a = new Nomination();
            a.ApplicantId = vm.ApplicantId;
            a.CreatedNom = DateTime.Now;
            a.LastEdited = DateTime.Now;
            a.OfferId = offerId;
            a.Offer = _db.Offer.Where(b => b.OfferId == offerId).Include(x=>x.University).ThenInclude(q=>q.Country).FirstOrDefault();
            a.StatusOfNomination = "Unknown";

            Models.Applicant applicant = _db.Applicant.Where(xa => xa.ApplicantId == a.ApplicantId).Include(xq => xq.ApplicationUser).ThenInclude(xe => xe.Country).Include(xw => xw.University).FirstOrDefault();

            _db.Nomination.Add(a);
            _db.SaveChanges();

            return RedirectToAction("profile", "dashboard", new { id = vm.ApplicantId });
        }

        [Autorizacija(false, false, true)]
        public IActionResult docs(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            EditDocsNomVM model = new EditDocsNomVM();
            model.NominationId = id;
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.ApplicantId = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault().ApplicantId;
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.ApplicantId).FirstOrDefault();
            return View("docs", model);
        }

        [HttpPost]
        public IActionResult docs(ViewNomVM model)
        {
            Nomination newNom = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).FirstOrDefault();
            Models.Applicant y = _db.Applicant.Where(x => x.ApplicantId == newNom.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();

            if (ModelState.IsValid)
            {
                string uniqueFileNameLA = null;
                string uniqueFileNamePASS = null;
                string uniqueFileNameWP = null;
                string uniqueFileNameCV = null;
                string uniqueFileNameEng = null;
                string uniqueFileNameToR = null;
                string uniqueFileNameML = null;
                string uniqueFileNameRL = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.LearningAgreement != null)
                {
                    string fileExt = Path.GetExtension(model.LearningAgreement.FileName);
                    if (fileExt == ".pdf")
                    {
                        // The image must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the inject
                        // HostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileNameLA = newNom.NominationId + "_" + model.LearningAgreement.FileName;
                        //uniqueFileNameLA = Guid.NewGuid().ToString() + "_" + model.LearningAgreement.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameLA);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.LearningAgreement.CopyTo(nesto);
                        nesto.Close();
                        newNom.LearningAgreement = uniqueFileNameLA;
                    }
                }

                if (model.WorkPlan != null)
                {
                    string fileExt = Path.GetExtension(model.WorkPlan.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameWP = newNom.NominationId + "_" + model.WorkPlan.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameWP);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.WorkPlan.CopyTo(nesto);
                        nesto.Close();
                        newNom.WorkPlan = uniqueFileNameWP;
                    }
                }

                if (model.CV != null)
                {
                    string fileExt = Path.GetExtension(model.CV.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameCV = newNom.NominationId + "_" + model.CV.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameCV);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.CV.CopyTo(nesto);
                        nesto.Close();
                        newNom.CV = uniqueFileNameCV;
                    }
                }

                if (model.Passport != null)
                {
                    string fileExt = Path.GetExtension(model.Passport.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNamePASS = newNom.NominationId + "_" + model.Passport.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNamePASS);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.Passport.CopyTo(nesto);
                        nesto.Close();
                        newNom.Passport = uniqueFileNamePASS;
                    }
                }

                if (model.EngProficiency != null)
                {
                    string fileExt = Path.GetExtension(model.EngProficiency.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameEng = newNom.NominationId + "_" + model.EngProficiency.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameEng);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.EngProficiency.CopyTo(nesto);
                        nesto.Close();
                        newNom.EngProficiency = uniqueFileNameEng;
                    }
                }

                if (model.TranscriptOfRecords != null)
                {
                    string fileExt = Path.GetExtension(model.TranscriptOfRecords.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameToR = newNom.NominationId + "_" + model.TranscriptOfRecords.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameToR);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.TranscriptOfRecords.CopyTo(nesto);
                        nesto.Close();
                        newNom.TranscriptOfRecords = uniqueFileNameToR;
                    }
                }

                if (model.MotivationLetter != null)
                {
                    string fileExt = Path.GetExtension(model.MotivationLetter.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameML = newNom.NominationId + "_" + model.MotivationLetter.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameML);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.MotivationLetter.CopyTo(nesto);
                        nesto.Close();
                        newNom.MotivationLetter = uniqueFileNameML;
                    }
                }

                if (model.ReferenceLetter != null)
                {
                    string fileExt = Path.GetExtension(model.ReferenceLetter.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameRL = newNom.NominationId + "_" + model.ReferenceLetter.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameRL);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.ReferenceLetter.CopyTo(nesto);
                        nesto.Close();
                        newNom.ReferenceLetter = uniqueFileNameRL;
                    }
                }

                newNom.LastEdited = DateTime.Now;
                _db.SaveChanges();

                return RedirectToAction("view", "nomination", new { id = newNom.NominationId });
            }
            return View();
        }

        [Autorizacija(false, false, true)]
        [HttpGet]
        public IActionResult view(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).Include(a => a.Offer).ThenInclude(b=>b.University).ThenInclude(c=>c.Country).FirstOrDefault();
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

        [Autorizacija(false, false, true)]
        public IActionResult comment(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Nomination.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("comment", model);
        }

        [HttpPost]
        public IActionResult comment(ViewNomVM model)
        {
            Nomination current = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).Include(a => a.University).FirstOrDefault();
           
            if(model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.ApplicantId = HttpContext.GetLoggedUser().Id;
                newComment.IonId = current.NominationId;

                newComment.CommentTime = DateTime.Now;

                var admini = _db.Administrator.Include(a => a.ApplicationUser).ToList();
                
                foreach(var x in admini)
                {
                    _notificationService.sendToAdmin(x.AdministratorId, HttpContext.GetLoggedUser().Id, new IRO_UNMO.App.Subscription.NotificationVM()
                    {
                        Message = model.NewComment,
                        Url = "/admin/nomination/view/" + current.NominationId
                    });
                }

                _db.Comment.Add(newComment);
                _db.SaveChanges();
            }
            else
            {
                return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
            }
            return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
        }

        [HttpPost]
        public IActionResult submit(ViewNomVM model)
        {
            Nomination current = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).Include(q=>q.Applicant).ThenInclude(w=>w.ApplicationUser).Include(a => a.University).FirstOrDefault();
            current.Finished = true;
            current.FinishedTime = DateTime.Now;

            var admini = _db.Administrator.Include(a => a.ApplicationUser).ToList();
            foreach (var x in admini)
            {
                _notificationService.sendToAdmin(x.AdministratorId, HttpContext.GetLoggedUser().Id, new IRO_UNMO.App.Subscription.NotificationVM()
                {
                    Message = "Nomination nr. " + current.NominationId + " has been submitted. Now you can review it!",
                    Url = "/admin/nomination/view/" + current.NominationId
                });
            }
            _db.SaveChanges();
            return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
        }
    }
}