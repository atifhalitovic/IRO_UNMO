﻿using System;
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
using IRO_UNMO.App.Subscription;
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
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

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

        private readonly INotification _notificationService;

        public NominationController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder, ILogger<DashboardController> logger, INotification notification)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _logger = logger;
            _userManagementHelper = new UserManagementHelper(_db);
            _notificationService = notification;
        }

        [Autorizacija(true, false, false)]
        public IActionResult cancel(int id)
        {
            Nomination current = _db.Nomination.Where(a => a.NominationId == id).FirstOrDefault();
            if (current != null)
            {
                _db.Nomination.Remove(current);
                _db.SaveChanges();
            }
            return RedirectToAction("review", "applicants", new { id = current.ApplicantId });
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

            Nomination mrki = _db.Nomination.Where(a => a.NominationId == id).Include(b => b.Applicant).ThenInclude(q => q.University).Include(b => b.Applicant).ThenInclude(c => c.ApplicationUser).ThenInclude(x => x.Country).FirstOrDefault();

            PdfDocument pdfDocument = new PdfDocument();

            //Add a page to the PDF document.

            PdfPage pdfPage = pdfDocument.Pages.Add();

            //Create a PDF Template.

            PdfTemplate template = new PdfTemplate(900, 900);

            //Draw a rectangle on the template graphics 

            template.Graphics.DrawRectangle(PdfBrushes.White, new Syncfusion.Drawing.RectangleF(0, 0, 900, 900));

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfFont font2 = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfFont font3 = new PdfStandardFont(PdfFontFamily.Helvetica, 16);

            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Draw a string using the graphics of the template.

            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 52); //dirao

            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

            //Load the PDF document

            FileStream imageStream = new FileStream("unmo_logo.png", FileMode.Open, FileAccess.Read);

            PdfImage image = new PdfBitmap(imageStream);

            //Draw the image in the header.

            header.Graphics.DrawImage(image, new PointF(75, 0), new SizeF(137, 52));
            header.Graphics.DrawString("Nomination number " + mrki.NominationId, font2, brush, 205, 12);

            //Add the header at the top.

            pdfDocument.Template.Top = header;

            template.Graphics.DrawString("Full name: ", font, brush, 32, 15);
            template.Graphics.DrawString(mrki.Applicant.ApplicationUser.Name + " " + mrki.Applicant.ApplicationUser.Surname, font, brush, 280, 15);

            template.Graphics.DrawString("Account created: ", font, brush, 32, 30);
            template.Graphics.DrawString(mrki.Applicant.CreatedProfile.ToString(), font, brush, 280, 30);

            template.Graphics.DrawString("Nomination created: ", font, brush, 32, 45);
            template.Graphics.DrawString(mrki.CreatedNom.ToString(), font, brush, 280, 45);

            template.Graphics.DrawString("Nomination submitted: ", font, brush, 32, 60);
            template.Graphics.DrawString(mrki.FinishedTime.ToString(), font, brush, 280, 60);

            template.Graphics.DrawString("Information", font3, brush, 10, 87);

            template.Graphics.DrawString("E-mail: ", font, brush, 32, 112);
            template.Graphics.DrawString(mrki.Applicant.ApplicationUser.Email.ToString(), font, brush, 280, 112);

            template.Graphics.DrawString("Phone number: ", font, brush, 32, 127);
            template.Graphics.DrawString(mrki.Applicant.ApplicationUser.PhoneNumber.ToString(), font, brush, 280, 127);

            template.Graphics.DrawString("Nationality: ", font, brush, 32, 142);
            template.Graphics.DrawString(mrki.Applicant.ApplicationUser.Country.Name.ToString(), font, brush, 280, 142);

            template.Graphics.DrawString("Faculty: ", font, brush, 32, 157);
            template.Graphics.DrawString(mrki.Applicant.FacultyName.ToString(), font, brush, 280, 157);

            template.Graphics.DrawString("University: ", font, brush, 32, 172);
            template.Graphics.DrawString(mrki.Applicant.University.Name.ToString(), font, brush, 280, 172);

            template.Graphics.DrawString("Student/staff: ", font, brush, 32, 187);
            template.Graphics.DrawString(mrki.Applicant.TypeOfApplication.ToString(), font, brush, 280, 187);

            var path1 = "";

            if (mrki.LearningAgreement != null)
            {
                path1 = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot\\uploads\\", mrki.LearningAgreement);
            }
            else if (mrki.WorkPlan != null)
            {
                path1 = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot\\uploads\\", mrki.WorkPlan);
            }

            var path2 = Path.Combine(
             Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.CV);

            var pathx = Path.Combine(
              Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.Passport);

            var path3 = Path.Combine(
             Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.EngProficiency);

            var path4 = Path.Combine(
             Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.TranscriptOfRecords);

            var path5 = Path.Combine(
             Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.MotivationLetter);

            var path6 = Path.Combine(
             Directory.GetCurrentDirectory(),
               "wwwroot\\uploads\\", mrki.ReferenceLetter);

            FileStream stream1 = new FileStream(path1, FileMode.Open, FileAccess.Read);

            FileStream stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read);

            FileStream streamx = new FileStream(pathx, FileMode.Open, FileAccess.Read);

            FileStream stream3 = new FileStream(path3, FileMode.Open, FileAccess.Read);

            FileStream stream4 = new FileStream(path4, FileMode.Open, FileAccess.Read);

            FileStream stream5 = new FileStream(path5, FileMode.Open, FileAccess.Read);


            // Creates a PDF stream for merging

            Stream[] streams = { stream1, stream2, stream3, streamx, stream4, stream5 };

            // Merges PDFDocument.

            PdfDocumentBase.Merge(pdfDocument, streams);

            ////Draw the template on the page graphics of the document.

            pdfPage.Graphics.DrawPdfTemplate(template, PointF.Empty);

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            stream.Position = 0;
            pdfDocument.Close(true);

            //Define the file name
            FileStreamResult nova = new FileStreamResult(stream, "nomination/pdf");
            string nameOfFile = "Nomination_" + mrki.NominationId + "_" + mrki.Applicant.ApplicationUser.Name + "_" + mrki.Applicant.ApplicationUser.Surname + ".pdf";
            nova.FileDownloadName = nameOfFile;
            return nova;
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
            var put = hosting.ContentRootPath;
            model.Nomination = _db.Nomination.Where(a => a.NominationId == id).Include(q => q.Offer).ThenInclude(a => a.University).ThenInclude(d => d.Country).FirstOrDefault();
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

            if (model.NewComment != null)
            {
                Comment newComment = new Comment();
                newComment.Message = model.NewComment;
                newComment.AdministratorId = id1;
                newComment.IonId = current.NominationId;
                newComment.CommentTime = DateTime.Now;

                _db.Comment.Add(newComment);
                _db.SaveChanges();
                _notificationService.sendToApplicant(current.ApplicantId, id1, new IRO_UNMO.App.Subscription.NotificationVM()
                {
                    Message = model.NewComment,
                    Url = "/applicant/nomination/view/" + current.NominationId
                });
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
            _notificationService.sendToApplicant(current.ApplicantId, HttpContext.GetLoggedUser().Id, new IRO_UNMO.App.Subscription.NotificationVM()
            {
                Message = "Your nomination status has been changed. New status is " + model.Nomination.StatusOfNomination + ".",
                Url = "/applicant/nomination/view/" + current.NominationId
            });
            return RedirectToAction("view", "nomination", new { id = model.Nomination.NominationId });
        }
    }
}