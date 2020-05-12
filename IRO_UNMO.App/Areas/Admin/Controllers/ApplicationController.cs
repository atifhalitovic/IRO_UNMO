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
    public class ApplicationController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        public ApplicationController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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

        public IActionResult docs(int id)
        {
            EditDocsVM model = new EditDocsVM();
            model.ApplicationId = id;
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Documents).FirstOrDefault();
            model.DocumentsId = _db.Application.Where(a => a.ApplicationId == id).FirstOrDefault().DocumentsId;
            model.Documents = _db.Documents.Where(a => a.DocumentsId == model.DocumentsId).FirstOrDefault();
            return View("docs", model);
        }

        [HttpPost]
        public IActionResult docs(EditDocsVM model)
        {
            Application newApp = _db.Application.Where(a => a.ApplicationId == model.ApplicationId).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault();
            Applicant y = _db.Applicant.Where(x => x.ApplicantId == newApp.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();
            Documents docs = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Documents).FirstOrDefault().Documents;

            if (ModelState.IsValid)
            {
                string uniqueFileNameLA = null;
                string uniqueFileNameCV = null;
                string uniqueFileNamePass = null;
                string uniqueFileNameEng = null;
                string uniqueFileNameToR= null;
                string uniqueFileNameML= null;
                string uniqueFileNameRL= null;
                string uniqueFileNameLAH= null;
                string uniqueFileNameCOA= null;
                string uniqueFileNameToRH= null;
                string uniqueFileNameExSR= null;
                string uniqueFileNameCOD=null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.LearningAgreement != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileNameLA = newApp.ApplicationId + "_" + model.LearningAgreement.FileName;
                    //uniqueFileNameLA = Guid.NewGuid().ToString() + "_" + model.LearningAgreement.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameLA);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.LearningAgreement.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.LearningAgreement = uniqueFileNameLA;
                }

                if (model.CV != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameCV = newApp.ApplicationId + "_" + model.CV.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameCV);
                    model.CV.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.CV = uniqueFileNameCV;
                }

                if (model.Passport != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNamePass = newApp.ApplicationId + "_" + model.Passport.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNamePass);
                    model.Passport.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.Passport = uniqueFileNamePass;
                }

                if (model.EngProficiency != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameEng = newApp.ApplicationId + "_" + model.EngProficiency.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameEng);
                    model.EngProficiency.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.EngProficiency = uniqueFileNameEng;
                }

                if (model.TranscriptOfRecords != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameToR = newApp.ApplicationId + "_" + model.TranscriptOfRecords.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameToR);
                    model.TranscriptOfRecords.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.TranscriptOfRecords = uniqueFileNameToR;
                }

                if (model.MotivationLetter != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameML = newApp.ApplicationId + "_" + model.MotivationLetter.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameML);
                    model.MotivationLetter.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.MotivationLetter = uniqueFileNameML;
                }

                if (model.ReferenceLetter != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameRL = newApp.ApplicationId + "_" + model.ReferenceLetter.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameRL);
                    model.ReferenceLetter.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.ReferenceLetter = uniqueFileNameRL;
                }

                if (model.LearningAgreementHost != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameLAH = newApp.ApplicationId + "_" + model.LearningAgreementHost.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameLAH);
                    model.LearningAgreementHost.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.LearningAgreementHost = uniqueFileNameLAH;
                }

                if (model.CertificateOfArrival != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameCOA = newApp.ApplicationId + "_" + model.CertificateOfArrival.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameCOA);
                    model.CertificateOfArrival.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.CertificateOfArrival = uniqueFileNameCOA;
                }

                if (model.CertificateOfDeparture != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameCOD = newApp.ApplicationId + "_" + model.CertificateOfDeparture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameCOD);
                    model.CertificateOfDeparture.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.CertificateOfDeparture = uniqueFileNameCOD;
                }

                if (model.StudentRecordSheet != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameExSR = newApp.ApplicationId + "_" + model.StudentRecordSheet.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameExSR);
                    model.StudentRecordSheet.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.StudentRecordSheet = uniqueFileNameExSR;
                }

                if (model.StudentTranscriptOfRecords != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameToRH = newApp.ApplicationId + "_" + model.StudentTranscriptOfRecords.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameToRH);
                    model.StudentTranscriptOfRecords.CopyTo(new FileStream(filePath, FileMode.Create));
                    docs.StudentTranscriptOfRecords = uniqueFileNameToRH;
                }

                _db.SaveChanges();

                return RedirectToAction("review", "applicants", new { id = newApp.ApplicantId });
            }

            return View();
        }

        public async Task<FileResult> DownloadFile(string fileName)
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


        [HttpGet]
        public IActionResult view(int id)
        {
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f=>f.Documents).Include(g=>g.Languages).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(x => x.ApplicantId == model.Application.ApplicantId).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(c=>c.University).FirstOrDefault();
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

        public IActionResult status(int id)
        {
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == id).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Application.ApplicantId).FirstOrDefault();
            return View("status", model);
        }

        [HttpPost]
        public IActionResult status(ViewAppVM model)
        {
            Application current = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).FirstOrDefault();
            current.StatusOfApplication = model.Application.StatusOfApplication;
            _db.SaveChanges();
            return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
        }
    }
}