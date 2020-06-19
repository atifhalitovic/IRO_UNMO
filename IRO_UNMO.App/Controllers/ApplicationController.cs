using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace IRO_UNMO.App.Controllers
{
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

        public IActionResult create(string id)
        {
            CreateNewAppVM model = new CreateNewAppVM();
            model.ApplicantId = id;
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == id).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).Include(xw => xw.University).FirstOrDefault();
            model.LastEdited = DateTime.Now;

            return View("create", model);
        }

        [HttpPost]
        public IActionResult create(CreateNewAppVM vm)
        {
            Application a = new Application();
            a.ApplicantId = vm.ApplicantId;
            a.CreatedApp = DateTime.Now;
            a.LastEdited = DateTime.Now;
            a.StatusOfApplication = "Unknown";

            Applicant applicant = _db.Applicant.Where(xa => xa.ApplicantId == vm.ApplicantId).Include(xq=>xq.ApplicationUser).ThenInclude(xe=>xe.Country).Include(xw=>xw.University).FirstOrDefault();

            Info newInfo = new Info();
            newInfo.CitizenshipId = applicant.ApplicationUser.CountryId;
            newInfo.Citizenship = _db.Country.Where(t => t.CountryId == newInfo.CitizenshipId).FirstOrDefault();
            a.Infos = newInfo;
            _db.Info.Add(newInfo);

            Contact newContact = new Contact();
            newContact.Email = applicant.ApplicationUser.Email;
            newContact.Telephone = applicant.ApplicationUser.PhoneNumber;
            newContact.Country = null;
            newContact.CountryId = null;
            a.Contacts = newContact;
            _db.Contact.Add(newContact);

            Language newLang = new Language();
            a.Languages = newLang;
            _db.Language.Add(newLang);

            HomeInstitution newHI = new HomeInstitution();
            newHI.OfficialName = _db.Applicant.Where(xy => xy.ApplicantId == vm.ApplicantId).FirstOrDefault().University.Name;
            newHI.LevelOfEducation = _db.Applicant.Where(xy => xy.ApplicantId == vm.ApplicantId).FirstOrDefault().StudyCycle;
            newHI.StudyProgramme = _db.Applicant.Where(xy => xy.ApplicantId == vm.ApplicantId).FirstOrDefault().StudyField;
            a.HomeInstitutions = newHI;
            _db.HomeInstitution.Add(newHI);

            Other newOther = new Other();
            a.Others = newOther;
            _db.Other.Add(newOther);

            Documents newDocs = new Documents();
            a.Documents = newDocs;
            _db.Documents.Add(newDocs);

            _db.Application.Add(a);
            _db.SaveChanges();

            return RedirectToAction("details", "account", new { id = vm.ApplicantId });
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

                return RedirectToAction("details", "account", new { id = newApp.ApplicantId });
            }

            return View();
        }

        [HttpGet]
        public IActionResult ViewDocs(int id)
        {
            ViewDocsVM model = new ViewDocsVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f => f.Documents).FirstOrDefault();
            return View("ViewDocs", model);
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

        public IActionResult edit(int id)
        {
            EditAppVM model = _userManagementHelper.prepApp();
            model.ApplicationId = id;
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f => f.Documents).Include(g => g.Languages).FirstOrDefault();
            return View("edit", model);
        }

        [HttpPost]
        public IActionResult edit(EditAppVM vm)
        {
            Application newApp = _db.Application.Where(a => a.ApplicationId == vm.ApplicationId).Include(b => b.Infos).ThenInclude(q=>q.Citizenship).Include(c => c.Contacts).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault();
            Applicant y = _db.Applicant.Where(x => x.ApplicantId == newApp.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();

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
            return RedirectToAction("details", "account", new { id = newApp.ApplicantId });
        }

        public IActionResult AddComment(int id)
        {
            ViewNomVM model = new ViewNomVM();
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Nomination.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("AddComment", model);
        }

        [HttpPost]
        public IActionResult AddComment(ViewNomVM model)
        {
            Nomination current = _db.Nomination.Where(a => a.NominationId == model.Nomination.NominationId).Include(a => a.University).FirstOrDefault();

            if (model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.ApplicantId = current.ApplicantId;
                newComment.IonId = current.NominationId;
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
    }
}