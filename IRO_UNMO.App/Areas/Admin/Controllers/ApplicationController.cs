using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using Syncfusion.Pdf;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf.Graphics;
using IRO_UNMO.Web.Helper;

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

        [HttpPost]
        public IActionResult pdf(int id)
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings settings = new WebKitConverterSettings();

            //Set WebKit path
            settings.WebKitPath = Path.Combine(hosting.ContentRootPath, "QtBinariesWindows");

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Convert URL to PDF

            Application mrki = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Applicant).ThenInclude(c => c.ApplicationUser).Include(d => d.Infos).ThenInclude(q => q.Citizenship).Include(e => e.HomeInstitutions).Include(f => f.Languages).Include(g => g.Contacts).ThenInclude(z => z.Country).Include(h => h.Documents).Include(i => i.Others).FirstOrDefault();

            PdfDocument pdfDocument = new PdfDocument();

            //Add a page to the PDF document.

            PdfPage pdfPage = pdfDocument.Pages.Add();
            PdfPage pdfPage2 = pdfDocument.Pages.Add();

            //Create a PDF Template.

            PdfTemplate template = new PdfTemplate(900, 900);
            PdfTemplate template2 = new PdfTemplate(900, 900);

            //Draw a rectangle on the template graphics 

            template.Graphics.DrawRectangle(PdfBrushes.White, new Syncfusion.Drawing.RectangleF(0, 0, 900, 900));
            template2.Graphics.DrawRectangle(PdfBrushes.White, new Syncfusion.Drawing.RectangleF(0, 0, 900, 900));

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont font2 = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfFont font3 = new PdfStandardFont(PdfFontFamily.Helvetica, 16);

            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Draw a string using the graphics of the template.


            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 52);

            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

            //Load the PDF document

            FileStream imageStream = new FileStream("unmo_logo.png", FileMode.Open, FileAccess.Read);

            PdfImage image = new PdfBitmap(imageStream);

            //Draw the image in the header.

            header.Graphics.DrawImage(image, new PointF(75, 0), new SizeF(137, 52));
            header.Graphics.DrawString("Application number " + mrki.ApplicationId, font2, brush, 205, 12);

            //Add the header at the top.

            pdfDocument.Template.Top = header;

            template.Graphics.DrawString("About", font3, brush, 10, 17);

            template.Graphics.DrawString("Full name: ", font, brush, 32, 42);
            template.Graphics.DrawString(mrki.Applicant.ApplicationUser.Name + " " + mrki.Applicant.ApplicationUser.Surname, font, brush, 280, 42);

            template.Graphics.DrawString("Account created: ", font, brush, 32, 57);
            template.Graphics.DrawString(mrki.Applicant.CreatedProfile.ToString(), font, brush, 280, 57);

            template.Graphics.DrawString("Application created: ", font, brush, 32, 72);
            template.Graphics.DrawString(mrki.CreatedApp.ToString(), font, brush, 280, 72);

            template.Graphics.DrawString("Application submitted: ", font, brush, 32, 87);
            template.Graphics.DrawString(mrki.FinishedTime.ToString(), font, brush, 280, 87);

            template.Graphics.DrawString("Information", font3, brush, 10, 107);

            template.Graphics.DrawString("Gender: ", font, brush, 32, 132);
            template.Graphics.DrawString(mrki.Infos.Gender, font, brush, 280, 132);

            template.Graphics.DrawString("Date of birth: ", font, brush, 32, 149);
            template.Graphics.DrawString(mrki.Infos.DateOfBirth.ToShortDateString(), font, brush, 280, 149);

            template.Graphics.DrawString("Place of birth: ", font, brush, 32, 166);
            template.Graphics.DrawString(mrki.Infos.PlaceOfBirth.ToString(), font, brush, 280, 166);

            template.Graphics.DrawString("Citizenship: ", font, brush, 32, 183);
            template.Graphics.DrawString(mrki.Infos.Citizenship.Name.ToString(), font, brush, 280, 183);

            template.Graphics.DrawString("Passport number: ", font, brush, 32, 200);
            template.Graphics.DrawString(mrki.Infos.PassportNumber.ToString(), font, brush, 280, 200);

            template.Graphics.DrawString("Passport issue date: ", font, brush, 32, 217);
            template.Graphics.DrawString(mrki.Infos.PassportIssueDate.ToShortDateString(), font, brush, 280, 217);

            template.Graphics.DrawString("Passport expiry date: ", font, brush, 32, 234);
            template.Graphics.DrawString(mrki.Infos.PassportExpiryDate.ToShortDateString(), font, brush, 280, 234);

            template.Graphics.DrawString("Contacts", font3, brush, 10, 259);

            template.Graphics.DrawString("E-mail: ", font, brush, 32, 284);
            template.Graphics.DrawString(mrki.Contacts.Email.ToString(), font, brush, 280, 284);

            template.Graphics.DrawString("Phone number: ", font, brush, 32, 301);
            template.Graphics.DrawString(mrki.Contacts.Telephone.ToString(), font, brush, 280, 301);

            template.Graphics.DrawString("Street name: ", font, brush, 32, 318);
            template.Graphics.DrawString(mrki.Contacts.StreetName.ToString(), font, brush, 280, 318);

            template.Graphics.DrawString("City, town, village: ", font, brush, 32, 335);
            template.Graphics.DrawString(mrki.Contacts.PlaceName.ToString(), font, brush, 280, 335);

            template.Graphics.DrawString("Postal code: ", font, brush, 32, 352);
            template.Graphics.DrawString(mrki.Contacts.PostalCode.ToString(), font, brush, 280, 352);

            template.Graphics.DrawString("Country of residence: ", font, brush, 32, 369);
            template.Graphics.DrawString(mrki.Contacts.Country.Name.ToString(), font, brush, 280, 369);

            template.Graphics.DrawString("Languages", font3, brush, 10, 394);

            template.Graphics.DrawString("Native language: ", font, brush, 32, 419);
            template.Graphics.DrawString(mrki.Languages.Native.ToString(), font, brush, 280, 419);

            template.Graphics.DrawString("First foreign language: ", font, brush, 32, 436);
            string ff = mrki.Languages.ForeignFirst + " | " + mrki.Languages.ForeignFirstProficiency;
            template.Graphics.DrawString(ff, font, brush, 280, 436);

            template.Graphics.DrawString("Second foreign language: ", font, brush, 32, 453);
            string sf = mrki.Languages.ForeignSecond + " | " + mrki.Languages.ForeignSecondProficiency;
            template.Graphics.DrawString(sf, font, brush, 280, 453);

            template.Graphics.DrawString("Third foreign language: ", font, brush, 32, 470);
            string tf = mrki.Languages.ForeignThird + " | " + mrki.Languages.ForeignThirdProficiency;
            template.Graphics.DrawString(tf, font, brush, 280, 470);

            template.Graphics.DrawString("Number of foreign experiences: ", font, brush, 32, 487);
            template.Graphics.DrawString(mrki.Languages.ForeignExperienceNumber.ToString(), font, brush, 280, 487);

            template.Graphics.DrawString("Home institution", font3, brush, 10, 512);

            template.Graphics.DrawString("University: ", font, brush, 32, 537);
            template.Graphics.DrawString(mrki.HomeInstitutions.OfficialName.ToString(), font, brush, 280, 537);

            //template.Graphics.DrawString("Faculty: ", font, brush, 5, 335);
            //template.Graphics.DrawString(mrki.Applicant.FacultyName.ToString(), font, brush, 280, 335);

            template.Graphics.DrawString("Department name: ", font, brush, 32, 554);
            template.Graphics.DrawString(mrki.HomeInstitutions.DepartmentName.ToString(), font, brush, 280, 554);

            template.Graphics.DrawString("Study programme: ", font, brush, 32, 571);
            template.Graphics.DrawString(mrki.HomeInstitutions.StudyProgramme.ToString(), font, brush, 280, 571);

            template.Graphics.DrawString("Study cycle: ", font, brush, 32, 588);
            template.Graphics.DrawString(mrki.Applicant.StudyCycle, font, brush, 280, 588);

            template.Graphics.DrawString("Current term/year: ", font, brush, 32, 605);
            template.Graphics.DrawString(mrki.HomeInstitutions.CurrentTermOrYear.ToString(), font, brush, 280, 605);

            template.Graphics.DrawString("Coordinator full name: ", font, brush, 32, 622);
            template.Graphics.DrawString(mrki.HomeInstitutions.CoordinatorFullName.ToString(), font, brush, 280, 622);

            template.Graphics.DrawString("Coordinator e-mail: ", font, brush, 32, 639);
            template.Graphics.DrawString(mrki.HomeInstitutions.CoordinatorEmail.ToString(), font, brush, 280, 639);

            template.Graphics.DrawString("Coordinator phone number: ", font, brush, 32, 656);
            template.Graphics.DrawString(mrki.HomeInstitutions.CoordinatorPhoneNum.ToString(), font, brush, 280, 656);

            template.Graphics.DrawString("Coordinator address: ", font, brush, 32, 673);
            template.Graphics.DrawString(mrki.HomeInstitutions.CoordinatorAddress.ToString(), font, brush, 280, 673);

            template.Graphics.DrawString("Coordinator position: ", font, brush, 32, 690);
            template.Graphics.DrawString(mrki.HomeInstitutions.CoordinatorPosition.ToString(), font, brush, 280, 690);

            template2.Graphics.DrawString("Others", font3, brush, 10, 5);

            template2.Graphics.DrawString("Medical info: ", font, brush, 32, 30);
            template2.Graphics.DrawString(mrki.Others.MedicalInfo.ToString(), font, brush, 280, 30);

            template2.Graphics.DrawString("Additional requests: ", font, brush, 32, 47);
            template2.Graphics.DrawString(mrki.Others.AdditionalRequests.ToString(), font, brush, 280, 47);

            var path1 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.LearningAgreement);


            var path2 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.CV);


            var path3 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.EngProficiency);


            var path4 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.TranscriptOfRecords);


            var path5 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.MotivationLetter);


            var path6 = Path.Combine(
             Directory.GetCurrentDirectory(),
             "wwwroot\\uploads\\", mrki.Documents.ReferenceLetter);

            FileStream stream1 = new FileStream(path1, FileMode.Open, FileAccess.Read);

            FileStream stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read);

            FileStream stream3 = new FileStream(path3, FileMode.Open, FileAccess.Read);

            FileStream stream4 = new FileStream(path4, FileMode.Open, FileAccess.Read);

            FileStream stream5 = new FileStream(path5, FileMode.Open, FileAccess.Read);

            //FileStream stream6 = new FileStream(path6, FileMode.Open, FileAccess.Read);

            // Creates a PDF stream for merging

            Stream[] streams = { stream1, stream2, stream3, stream4, stream5 };// stream6 };

            // Merges PDFDocument.

            PdfDocumentBase.Merge(pdfDocument, streams);

            ////Draw the template on the page graphics of the document.

            pdfPage.Graphics.DrawPdfTemplate(template, PointF.Empty);
            pdfPage2.Graphics.DrawPdfTemplate(template2, PointF.Empty);

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            stream.Position = 0;
            pdfDocument.Close(true);

            //Define the file name
            FileStreamResult nova = new FileStreamResult(stream, "application/pdf");
            string nameOfFile = "Application_" + mrki.ApplicationId + "_" + mrki.Applicant.ApplicationUser.Name + "_" + mrki.Applicant.ApplicationUser.Surname + ".pdf";
            nova.FileDownloadName = nameOfFile;
            return nova;
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
            Models.Applicant y = _db.Applicant.Where(x => x.ApplicantId == newApp.ApplicantId).Include(b => b.ApplicationUser).ThenInclude(c => c.Country).FirstOrDefault();
            Documents docs = _db.Application.Where(a => a.ApplicationId == newApp.ApplicationId).Include(b => b.Documents).FirstOrDefault().Documents;

            if (ModelState.IsValid)
            {
                string uniqueFileNameLA = null;
                string uniqueFileNameCV = null;
                string uniqueFileNamePass = null;
                string uniqueFileNameEng = null;
                string uniqueFileNameToR = null;
                string uniqueFileNameML = null;
                string uniqueFileNameRL = null;
                string uniqueFileNameLAH = null;
                string uniqueFileNameCOA = null;
                string uniqueFileNameToRH = null;
                string uniqueFileNameExSR = null;
                string uniqueFileNameCOD = null;

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
                    string fileExt = Path.GetExtension(uniqueFileNameLA);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.LearningAgreement.CopyTo(nesto);
                    nesto.Close();
                    docs.LearningAgreement = uniqueFileNameLA;
                }

                if (model.CV != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameCV = newApp.ApplicationId + "_" + model.CV.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameCV);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.CV.CopyTo(nesto);
                    nesto.Close();
                    docs.CV = uniqueFileNameCV;
                }

                if (model.Passport != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNamePass = newApp.ApplicationId + "_" + model.Passport.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNamePass);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.Passport.CopyTo(nesto);
                    nesto.Close();
                    docs.Passport = uniqueFileNamePass;
                }

                if (model.EngProficiency != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameEng = newApp.ApplicationId + "_" + model.EngProficiency.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameEng);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.EngProficiency.CopyTo(nesto);
                    nesto.Close();
                    docs.EngProficiency = uniqueFileNameEng;
                }

                if (model.TranscriptOfRecords != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameToR = newApp.ApplicationId + "_" + model.TranscriptOfRecords.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameToR);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.TranscriptOfRecords.CopyTo(nesto);
                    nesto.Close();
                    docs.TranscriptOfRecords = uniqueFileNameToR;
                }

                if (model.MotivationLetter != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameML = newApp.ApplicationId + "_" + model.MotivationLetter.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameML);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.MotivationLetter.CopyTo(nesto);
                    nesto.Close();
                    docs.MotivationLetter = uniqueFileNameML;
                }

                if (model.ReferenceLetter != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                    uniqueFileNameRL = newApp.ApplicationId + "_" + model.ReferenceLetter.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileNameRL);
                    var nesto = new FileStream(filePath, FileMode.Create);
                    model.ReferenceLetter.CopyTo(nesto);
                    nesto.Close();
                    docs.ReferenceLetter = uniqueFileNameRL;
                }

                if (model.LearningAgreementHost != null)
                {
                    string fileExt = Path.GetExtension(model.LearningAgreementHost.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameLAH = newApp.ApplicationId + "_" + model.LearningAgreementHost.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameLAH);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.LearningAgreementHost.CopyTo(nesto);
                        nesto.Close();
                        docs.LearningAgreementHost = uniqueFileNameLAH;
                    }
                }

                if (model.CertificateOfArrival != null)
                {
                    string fileExt = Path.GetExtension(model.CertificateOfArrival.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameCOA = newApp.ApplicationId + "_" + model.CertificateOfArrival.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameCOA);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.CertificateOfArrival.CopyTo(nesto);
                        nesto.Close();
                        docs.CertificateOfArrival = uniqueFileNameCOA;
                    }
                }

                if (model.CertificateOfDeparture != null)
                {
                    string fileExt = Path.GetExtension(model.CertificateOfDeparture.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameCOD = newApp.ApplicationId + "_" + model.CertificateOfDeparture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameCOD);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.CertificateOfDeparture.CopyTo(nesto);
                        nesto.Close();
                        docs.CertificateOfDeparture = uniqueFileNameCOD;
                    }
                }

                if (model.StudentRecordSheet != null)
                {
                    string fileExt = Path.GetExtension(model.StudentRecordSheet.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameExSR = newApp.ApplicationId + "_" + model.StudentRecordSheet.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameExSR);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.StudentRecordSheet.CopyTo(nesto);
                        nesto.Close();
                        docs.StudentRecordSheet = uniqueFileNameExSR;
                    }
                }

                if (model.StudentTranscriptOfRecords != null)
                {
                    string fileExt = Path.GetExtension(model.StudentTranscriptOfRecords.FileName);
                    if (fileExt == ".pdf")
                    {
                        string uploadsFolder = Path.Combine(hosting.WebRootPath, "uploads");
                        uniqueFileNameToRH = newApp.ApplicationId + "_" + model.StudentTranscriptOfRecords.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileNameToRH);
                        var nesto = new FileStream(filePath, FileMode.Create);
                        model.StudentTranscriptOfRecords.CopyTo(nesto);
                        nesto.Close();
                        docs.StudentTranscriptOfRecords = uniqueFileNameToRH;
                    }
                }

                _db.SaveChanges();
                return RedirectToAction("docs", "application", new { id = newApp.ApplicationId });
            }

            return View();
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
                stream.Close();
            }

            memory.Position = 0;
            return File(memory, MediaTypeNames.Application.Octet, Path.GetFileName(path));
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

            if (fileType == "LearningAgreementHost")
            {
                docs.LearningAgreementHost = null;
            }
            if (fileType == "CertificateOfArrival")
            {
                docs.CertificateOfArrival = null;
            }
            if (fileType == "StudentTranscriptOfRecords")
            {
                docs.StudentTranscriptOfRecords = null;
            }
            if (fileType == "CertificateOfDeparture")
            {
                docs.CertificateOfDeparture = null;
            }
            if (fileType == "StudentRecordSheet")
            {
                docs.StudentRecordSheet = null;
            }
            _db.SaveChanges();

            return RedirectToAction("docs", "application", new { id });
        }

        [Autorizacija(true, false, false)]
        [HttpGet]
        public IActionResult view(int id)
        {
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).Include(f => f.Documents).Include(g => g.Languages).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(x => x.ApplicantId == model.Application.ApplicantId).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(c => c.University).FirstOrDefault();
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

        [Autorizacija(true, false, false)]
        public IActionResult comment(int id)
        {
            ViewAppVM model = new ViewAppVM();
            model.Application = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).FirstOrDefault();
            model.Applicant = _db.Applicant.Where(a => a.ApplicantId == model.Application.ApplicantId).FirstOrDefault();
            model.Comments = _db.Comment.Where(x => x.IonId == id).ToList();
            return View("comment", model);
        }

        [HttpPost]
        public IActionResult comment(ViewAppVM model)
        {
            Application current = _db.Application.Where(a => a.ApplicationId == model.Application.ApplicationId).Include(a => a.Applicant).ThenInclude(b => b.ApplicationUser).FirstOrDefault();

            if (model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.AdministratorId = HttpContext.GetLoggedUser().Id;
                newComment.IonId = current.ApplicationId;
                newComment.CommentTime = DateTime.Now;

                _db.Comment.Add(newComment);
                _db.SaveChanges();
            }
            else
            {
                return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
            }
            return RedirectToAction("view", "application", new { id = model.Application.ApplicationId });
        }
    }
}