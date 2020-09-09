using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.Services;
using IRO_UNMO.App.ViewModels;
using IRO_UNMO.Util;
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IRO_UNMO.App.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _db;
        private IConfiguration _configuration;

        private readonly IHostingEnvironment hosting;
        private UserManagementHelper _userManagementHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UrlEncoder _urlEncoder;

        public AccountController(ApplicationDbContext db, IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, UrlEncoder urlEncoder, ILogger<AccountController> logger, IConfiguration config)
        {
            _db = db;
            hosting = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _logger = logger;
            _configuration = config;
            _userManagementHelper = new UserManagementHelper(_db);
        }

        public string ReturnUrl { get; set; }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginVM { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ApplicationUser user = _db.Users.SingleOrDefault
                (i => i.UniqueCode == model.UniqueCode);// && i.LozinkaHash == PasswordSettings.GetHash(model.Lozinka, Convert.FromBase64String(i.LozinkaSalt)));

            if (user == null)
            {
                TempData["errorMessage"] = "Account info is not valid.";
                return View(model);
            }
            var userRole = _userManager.GetRolesAsync(user).Result.Single();

            if (userRole == "Administrator")
            {
                HttpContext.SetLoggedUser(user, true);
                return RedirectToAction("index", "dashboard", new { area = "admin" });
            }
            else if (userRole == "IncomingApplicant")
            {
                HttpContext.SetLoggedUser(user, true);
                TempData["applicantId"] = user.Id;
                return RedirectToAction("profile", "dashboard", new { area = "applicant", @id = user.Id });
            }
            else if (userRole == "OutgoingApplicant")
            {
                HttpContext.SetLoggedUser(user, true);
                TempData["applicantId"] = user.Id;
                return RedirectToAction("profile", "dashboard", new { area = "applicant", @id = user.Id });
            }

            TempData["errorMessage"] = "Account info is not valid.";
            return View(model);
        }

        public IActionResult logout()
        {
            ApplicationUser loggedUser = HttpContext.GetLoggedUser();

            if (loggedUser == null)
                return RedirectToAction("login", "account");

            string trenutniToken = HttpContext.GetTrenutniToken();

            Token token = _db.Token.SingleOrDefault
                (x => x.ApplicationUserId == loggedUser.Id && x.Value == trenutniToken);

            _db.Token.Remove(token);

            List<Token> tokeni = _db.Token.Where
                (x => (DateTime.Now - x.Created).TotalHours >= 24 && x.ApplicationUserId == loggedUser.Id).ToList();

            foreach (Token t in tokeni)
            {
                _db.Token.Remove(t);
            }

            loggedUser.LastLogin = DateTime.Now;
            _db.SaveChanges();

            Response.Cookies.Delete("loggedUser");

            return RedirectToAction("logout", "account");
        }

        [AllowAnonymous]
        public IActionResult type()
        {
            return View("type");
        }

        [AllowAnonymous]
        public IActionResult incoming()
        {
            UsersVM model = _userManagementHelper.prepUser();
            return View("incoming", model);
        }

        public int brojac = 0;
        [HttpPost]
        public async Task<IActionResult> incoming(UsersVM model)
        {
            if (_db.Users.Any(i => i.Email == model.Email))
            {
                TempData["errorMessage"] = "E-mail you choosed is currently in use. Please use another.";
                return RedirectToAction("incoming");
            }

            bool x = await _roleManager.RoleExistsAsync("IncomingApplicant");

            if (!x)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "IncomingApplicant"
                });
            }

            //password must be strong enough in order for userManager.CreateAsync to work!!!
            string password = "myP@ssW0r@d123";

            var brojKorisnika = _db.Users.Count();

            brojac = ++brojKorisnika;

            ApplicationUser user = new ApplicationUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CountryId = model.CountryId,
                UserName = model.Name.ToLower() + '.' + model.Surname.ToLower(),
                UniqueCode = GetRandomizedString(brojac),
                LastLogin = DateTime.Now
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, "IncomingApplicant");

            Applicant applicant = new Applicant
            {
                ApplicantId = user.Id,
                CreatedProfile = DateTime.Now,
                UniversityId = model.UniversityId,
                StudyCycle = model.StudyCycle,
                StudyField = model.StudyField,
                Verified = false,
                TypeOfApplication = model.TypeOfApplication
            };

            _db.Applicant.Add(applicant);
            _db.SaveChanges();

            string welcome = "Thank you for the registration at IRO Dzemal Bijedic University of Mostar system!\n";
            string thanks = "We wish you the best of luck for your application. Please follow the rules!\n";
            string contact = "In case of any problems you can contact us at international@unmo.ba \n";
            string msg = welcome + "Your unique code is: " + user.UniqueCode + "\nPlease login with your code. " + thanks + contact;
            EmailSettings.SendEmail(_configuration, user.Name + " " + user.Surname, user.Email, "Login info", msg);

            TempData["successMessage"] = "You have successfully registered! Now you can log in.";
            return RedirectToAction("login", "account");
        }

        [AllowAnonymous]
        public IActionResult outgoing()
        {
            UsersVM model = _userManagementHelper.prepUser();
            return View("outgoing", model);
        }

        [HttpPost]
        public async Task<IActionResult> outgoing(UsersVM model)
        {
            if (_db.Users.Any(i => i.Email == model.Email))
            {
                TempData["errorMessage"] = "E-mail you choosed is currently in use. Please use another.";
                return RedirectToAction("outgoing");
            }

            if (ModelState.IsValid)
            {
                bool x = await _roleManager.RoleExistsAsync("OutgoingApplicant");

                if (!x)
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "OutgoingApplicant"
                    });
                }

                //password must be strong enough in order for userManager.CreateAsync to work!!!
                string password = "myP@ssW0r@d123";

                var brojKorisnika = _db.Users.Count();

                brojac = ++brojKorisnika;

                ApplicationUser user = new ApplicationUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CountryId = model.CountryId,
                    UserName = model.Name.ToLower() + '.' + model.Surname.ToLower(),
                    UniqueCode = GetRandomizedString(brojac),
                    LastLogin = DateTime.Now
                };

                await _userManager.CreateAsync(user, password);

                await _userManager.AddToRoleAsync(user, "OutgoingApplicant");

                Applicant applicant = new Applicant
                {
                    ApplicantId = user.Id,
                    ApplicationUser = user,
                    CreatedProfile = DateTime.Now,
                    UniversityId = 2,
                    FacultyName = model.FacultyName,
                    TypeOfApplication = model.TypeOfApplication,
                    StudyCycle = model.StudyCycle,
                    StudyField = model.StudyField,
                    Verified = false
                };

                _db.Applicant.Add(applicant);
                _db.SaveChanges();

                string welcome = "Thank you for the registration at IRO Dzemal Bijedic University of Mostar system!\n";
                string thanks = "We wish you the best of luck for your nomination. Please follow the rules!\n";
                string contact = "In case of any problems you can contact us at international@unmo.ba \n";
                string msg = welcome + "Your unique code is: " + user.UniqueCode + "\nPlease login with your code. " + thanks + contact;
                EmailSettings.SendEmail(_configuration, user.Name + " " + user.Surname, user.Email, "Login info", msg);

                TempData["successMessage"] = "You have successfully registered! Now you can log in.";
                return RedirectToAction("login", "account");
            }
            TempData["errorMessage"] = "Something went wrong, please try again.";
            return RedirectToAction("outgoing", "account");
        }

        public IActionResult admin()
        {
            UsersVM model = _userManagementHelper.prepUser();
            return View("admin", model);
        }

        [HttpPost]
        public async Task<IActionResult> admin(UsersVM model)
        {
            bool x = await _roleManager.RoleExistsAsync("Administrator");

            if (!x)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Administrator"
                });
            }

            //password must be strong enough in order for userManager.CreateAsync to work!!!
            string password = "myP@ssW0r@d123";

            var brojKorisnika = _db.Users.Count();

            brojac = ++brojKorisnika;

            ApplicationUser user = new ApplicationUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CountryId = model.CountryId,
                UserName = model.Name.ToLower() + '.' + model.Surname.ToLower(),
                UniqueCode = GetRandomizedString(brojac),
                LastLogin = DateTime.Now
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, "Administrator");

            Administrator admin = new Administrator
            {
                AdministratorId = user.Id,
                CreatedProfile = DateTime.Now,
            };

            _db.Administrator.Add(admin);
            _db.SaveChanges();

            return RedirectToAction("index", "dashboard");
        }

        // Returns a string that is the encoded representation of the input number, and a random value 
        static String GetRandomizedString(Int32 input)
        {
            Int32 uniqueLength = 6; // Length of the unique string (based on the input) 
            Int32 randomLength = 4; // Length of the random string (based on the RNG) 
            String uniqueString;
            String randomString;
            StringBuilder resultString = new StringBuilder(uniqueLength + randomLength);

            // This might not be the best way of seeding the RNG, so feel free to replace it with better alternatives. 
            // Here, the seed is based on the ratio of the current time and the input number. The ratio is flipped 
            // around (i.e. it is either M/N or N/M) to ensure an integer is returned. 
            // Casting an expression with Ticks (Long) to Int32 results in truncation, which is fine since this is 
            // only a seed for an RNG 
            Random randomizer = new Random(
                    (Int32)(
                        DateTime.Now.Ticks + (DateTime.Now.Ticks > input ? DateTime.Now.Ticks / (input + 1) : input / DateTime.Now.Ticks)
                    )
                );

            // Get a random number and encode it as a string, limit its length to 'randomLength' 
            randomString = EncodeInt32AsString(randomizer.Next(1, Int32.MaxValue), randomLength);
            // Encode the input number and limit its length to 'uniqueLength' 
            uniqueString = EncodeInt32AsString(input, uniqueLength);

            // For debugging/display purposes alone: show the 2 constituent parts 
            resultString.AppendFormat("{0}\t {1}\t ", uniqueString, randomString);

            // Take successive characters from the unique and random strings and 
            // alternate them in the output 
            for (Int32 i = 0; i < Math.Min(uniqueLength, randomLength); i++)
            {
                resultString.AppendFormat("{0}{1}", uniqueString[i], randomString[i]);
            }
            resultString.Append((uniqueLength < randomLength ? randomString : uniqueString).Substring(Math.Min(uniqueLength, randomLength)));

            String newStringTest = resultString.ToString();

            String newString = newStringTest.Substring(14);

            return newString.ToString();
        }

        static String EncodeInt32AsString(Int32 input, Int32 maxLength = 0)
        {
            // List of characters allowed in the target string 
            Char[] allowedList = new Char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z' };
            Int32 allowedSize = allowedList.Length;
            StringBuilder result = new StringBuilder(input.ToString().Length);

            Int32 moduloResult;
            while (input > 0)
            {
                moduloResult = input % allowedSize;
                input /= allowedSize;
                result.Insert(0, allowedList[moduloResult]);
            }

            if (maxLength > result.Length)
            {
                result.Insert(0, new String(allowedList[0], maxLength - result.Length));
            }

            if (maxLength > 0)
                return result.ToString().Substring(0, maxLength);
            else
                return result.ToString();
        }

        [AllowAnonymous]
        public IActionResult denied()
        {
            return View("denied");
        }
        [AllowAnonymous]
        public IActionResult code()
        {
            ForgotUniqueCodeVM model = new ForgotUniqueCodeVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult code(ForgotUniqueCodeVM model)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                TempData["errorMessage"] = "There is no user with this email. Please try again.";
                return View(model);
            }

            var brojKorisnika = _db.Users.Count();
            brojac = ++brojKorisnika;

            user.UniqueCode = GetRandomizedString(brojac);

            _db.SaveChanges();

            string msg = "Your new unique code is: " + user.UniqueCode + "\nNow you can login with the new code.";
            EmailSettings.SendEmail(_configuration, user.Name + " " + user.Surname, user.Email, "New login info", msg);

            TempData["successMessage"] = "You have successfully changed your code! Check for it at your email and you can log in.";
            return RedirectToAction("login", "account");
        }

        [HttpGet]
        public IActionResult details(string id)
        {
            ProfileVM vm = new ProfileVM
            {
                Applicant = _db.Applicant.Where(x => x.ApplicantId == id).Include(a => a.ApplicationUser).ThenInclude(b => b.Country).Include(b => b.University).FirstOrDefault(),
                Application = _db.Application.Where(a => a.ApplicantId == id).Include(b => b.Infos).ThenInclude(q => q.Citizenship).Include(c => c.Contacts).ThenInclude(q => q.Country).Include(d => d.HomeInstitutions).Include(e => e.Others).FirstOrDefault(),
                Nominations = _db.Nomination.Where(a => a.ApplicantId == id).Include(b => b.University).ToList()
            };
            return View(vm);
        }
    }
}