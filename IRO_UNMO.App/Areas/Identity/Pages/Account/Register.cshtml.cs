using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Areas.Identity.Pages.Account
{   
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string Surname { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            public string ApplicantType { get; set; }
            [Required]
            public string StudyCycle { get; set; }
            [Required]
            public string StudyField { get; set; }
            [Required]
            public int NationalityId { get; set; }
            [Required]
            public int UniversityId { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //bool x = await _roleManager.RoleExistsAsync("IncomingApplicant");

                //if (!x)
                //{
                //    await _roleManager.CreateAsync(new IdentityRole
                //    {
                //        Name = "IncomingApplicant"
                //    });
                //}

                //password must be strong enough in order for userManager.CreateAsync to work!!!
                string password = "myP@ssW0r@d123";

                var brojKorisnika = _db.Users.Count();

                //brojac = ++brojKorisnika;

                ApplicationUser user = new ApplicationUser
                {
                    Name = Input.Name,
                    Surname = Input.Surname,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    CountryId = Input.NationalityId,
                    UserName = Input.Name.ToLower() + '.' + Input.Surname.ToLower(),
                    UniqueCode = "xxxx", //GetRandomizedString(brojac),
                    LastLogin = DateTime.Now
                };

                await _userManager.CreateAsync(user, password);

                await _userManager.AddToRoleAsync(user, "IncomingApplicant");

                IRO_UNMO.App.Models.Applicant a = new IRO_UNMO.App.Models.Applicant
                {
                    ApplicantId = user.Id,
                    CreatedProfile = DateTime.Now,
                    UniversityId = Input.UniversityId,
                    StudyCycle = Input.StudyCycle,
                    StudyField = Input.StudyField,
                    Verified = false,
                    TypeOfApplication = Input.ApplicantType
                };

                _db.Applicant.Add(a);
                _db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                IdentityResult result = null;
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
