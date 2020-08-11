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
    public class ApplicationController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ApplicationDbContext _db;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly INotification _notificationService;

        public ApplicationController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
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

        [Autorizacija(false, true, false)]
        public IActionResult docs(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
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
            Models.Applicant y = _db.Applicant.Where(x => x.ApplicantId == newApp.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();
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
                        uniqueFileNameLA = newApp.ApplicationId + "_" + model.LearningAgreement.FileName;
                        //uniqueFileNameLA = Guid.NewGuid().ToString() + "_" + model.LearningAgreement.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameLA);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        model.LearningAgreement.CopyTo(nesto);
                        nesto.Close();
                        docs.LearningAgreement = uniqueFileNameLA;
                    }
                }

                if (model.CV != null)
                {
                    string fileExt = Path.GetExtension(model.CV.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameCV = newApp.ApplicationId + "_" + model.CV.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameCV);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.CV.CopyTo(nesto);
                        nesto.Close();
                        docs.CV = uniqueFileNameCV;
                    }
                }

                if (model.Passport != null)
                {
                    string fileExt = Path.GetExtension(model.Passport.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNamePass = newApp.ApplicationId + "_" + model.Passport.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNamePass);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.Passport.CopyTo(nesto);
                        nesto.Close();
                        docs.Passport = uniqueFileNamePass;
                    }
                }

                if (model.EngProficiency != null)
                {
                    string fileExt = Path.GetExtension(model.EngProficiency.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameEng = newApp.ApplicationId + "_" + model.EngProficiency.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameEng);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.EngProficiency.CopyTo(nesto);
                        nesto.Close();
                        docs.EngProficiency = uniqueFileNameEng;
                    }
                }

                if (model.TranscriptOfRecords != null)
                {
                    string fileExt = Path.GetExtension(model.TranscriptOfRecords.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameToR = newApp.ApplicationId + "_" + model.TranscriptOfRecords.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameToR);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.TranscriptOfRecords.CopyTo(nesto);
                        nesto.Close();
                        docs.TranscriptOfRecords = uniqueFileNameToR;
                    }
                }

                if (model.MotivationLetter != null)
                {
                    string fileExt = Path.GetExtension(model.MotivationLetter.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameML = newApp.ApplicationId + "_" + model.MotivationLetter.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameML);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.MotivationLetter.CopyTo(nesto);
                        nesto.Close();
                        docs.MotivationLetter = uniqueFileNameML;
                    }
                }

                if (model.ReferenceLetter != null)
                {
                    string fileExt = Path.GetExtension(model.ReferenceLetter.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameRL = newApp.ApplicationId + "_" + model.ReferenceLetter.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameRL);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.ReferenceLetter.CopyTo(nesto);
                        nesto.Close();
                        docs.ReferenceLetter = uniqueFileNameRL;
                    }
                }

                _db.SaveChanges();

                //return RedirectToAction("details", "home", new { id = newApp.ApplicantId });
                return RedirectToAction("docs", "application", new { id = newApp.ApplicationId });
            }
            return View();
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

            var docs = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Documents).FirstOrDefault().Documents;

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
                docs.CV = null;
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

            return RedirectToAction("docs", "application", new { id });
        }

        public async Task<FileResult> download(string fileName)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;

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

        [Autorizacija(false, true, false)]
        [HttpGet]
        public IActionResult view(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f=>f.Documents).Include(g=>g.Languages).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(x => x.ApplicantId == model.Application.ApplicantId).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(c=>c.University).FirstOrDefault();
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

        [Autorizacija(false, true, false)]
        public IActionResult status(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
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

        [Autorizacija(false, true, false)]
        public IActionResult edit(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            EditAppVM model = _userManagementHelper.prepApp();
            model.ApplicationId = id;
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f => f.Documents).Include(g => g.Languages).FirstOrDefault();
            return View("edit", model);
        }

        [HttpPost]
        public IActionResult edit(EditAppVM vm)
        {
            Application newApp = _db.Application.Where(a => a.ApplicationId == vm.ApplicationId).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault();
            Models.Applicant y = _db.Applicant.Where(x => x.ApplicantId == newApp.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();

            Info info = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Infos).FirstOrDefault().Infos;
            info.Gender = vm.Application.Infos.Gender;
            //info.CitizenshipId = vm.Application.Applicant.ApplicationUser.CountryId;
            //info.CitizenshipId = vm.Application.Infos.CitizenshipId;
            info.SecondCitizenshipId = vm.Application.Infos.SecondCitizenshipId;
            info.DateOfBirth = vm.Application.Infos.DateOfBirth;
            info.PlaceOfBirth = vm.Application.Infos.PlaceOfBirth;
            info.PassportNumber = vm.Application.Infos.PassportNumber;
            info.PassportIssueDate = vm.Application.Infos.PassportIssueDate;
            info.PassportExpiryDate = vm.Application.Infos.PassportExpiryDate;
            newApp.LastEdited = DateTime.Now;

            Contact contact = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Contacts).FirstOrDefault().Contacts;
            //contact.Email = y.ApplicationUser.Email;
            //contact.Telephone = y.ApplicationUser.PhoneNumber;
            contact.StreetName = vm.Application.Contacts.StreetName;
            contact.PlaceName = vm.Application.Contacts.PlaceName;
            contact.PostalCode = vm.Application.Contacts.PostalCode;
            contact.CountryId = vm.Application.Contacts.CountryId;
            newApp.LastEdited = DateTime.Now;

            Language lang = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Languages).FirstOrDefault().Languages;
            lang.Native = vm.Application.Languages.Native;
            lang.ForeignFirst = vm.Application.Languages.ForeignFirst;
            lang.ForeignFirstProficiency = vm.Application.Languages.ForeignFirstProficiency;
            lang.ForeignSecond = vm.Application.Languages.ForeignSecond;
            lang.ForeignSecondProficiency = vm.Application.Languages.ForeignSecondProficiency;
            lang.ForeignThird = vm.Application.Languages.ForeignThird;
            lang.ForeignThirdProficiency = vm.Application.Languages.ForeignThirdProficiency;
            lang.ForeignExperienceNumber = vm.Application.Languages.ForeignExperienceNumber;
            newApp.LastEdited = DateTime.Now;


            HomeInstitution homeInst = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.HomeInstitutions).FirstOrDefault().HomeInstitutions;
            //homeInst.OfficialName = _db.Applicant.Where(a => a.ApplicantId == newApp.ApplicantId).FirstOrDefault().University.Name;
            //homeInst.OfficialName = vm.Application.HomeInstitutions.OfficialName;
            homeInst.DepartmentName = vm.Application.HomeInstitutions.DepartmentName;
            //homeInst.LevelOfEducation = vm.Application.HomeInstitutions.LevelOfEducation;
            homeInst.CurrentTermOrYear = vm.Application.HomeInstitutions.CurrentTermOrYear;
            //homeInst.StudyProgramme = vm.Application.HomeInstitutions.StudyProgramme;
            homeInst.CoordinatorFullName = vm.Application.HomeInstitutions.CoordinatorFullName;
            homeInst.CoordinatorPosition = vm.Application.HomeInstitutions.CoordinatorPosition;
            homeInst.CoordinatorEmail = vm.Application.HomeInstitutions.CoordinatorEmail;
            homeInst.CoordinatorPhoneNum = vm.Application.HomeInstitutions.CoordinatorPhoneNum;
            homeInst.CoordinatorAddress = vm.Application.HomeInstitutions.CoordinatorAddress;

            Other other = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Others).FirstOrDefault().Others;
            other.MedicalInfo = vm.Application.Others.MedicalInfo;
            other.AdditionalRequests = vm.Application.Others.AdditionalRequests;

            _db.SaveChanges();
            return RedirectToAction("profile", "dashboard", new { id = newApp.ApplicantId });
        }

        [Autorizacija(false, true, false)]
        public IActionResult comment(int id)
        {
            TempData["applicantId"] = HttpContext.GetLoggedUser().Id;
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Application.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("add", model);
        }

        [HttpPost]
        public IActionResult comment(ViewAppVM model)
        {
            Application current = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).FirstOrDefault();

            if (model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.ApplicantId = HttpContext.GetLoggedUser().Id;
                newComment.IonId = current.ApplicationId;
                newComment.CommentTime = DateTime.Now;

                var admini = _db.Administrator.Include(a => a.ApplicationUser).ToList();
                foreach (var x in admini)
                {
                    _notificationService.sendToAdmin(x.AdministratorId, HttpContext.GetLoggedUser().Id, new IRO_UNMO.App.Subscription.NotificationVM()
                    {
                        Message = model.NewComment,
                        Url = "/admin/application/view/" + current.ApplicationId
                    });
                }

                _db.Comment.Add(newComment);
                _db.SaveChanges();
            }
            else
            {
                return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
            }
            return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
        }

        [HttpPost]
        public IActionResult submit(ViewAppVM model)
        {
            Application current = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f => f.Documents).Include(g => g.Languages).FirstOrDefault();
            current.Finished = true;
            current.FinishedTime = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
        }
    }
}