using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;

namespace IRO_UNMO.Web.Helper
{
    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(bool admin, bool incoming, bool outgoing) : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { admin, incoming, outgoing };
        }
    }

    public class MyAuthorizeImpl : IAsyncActionFilter
    {
        private readonly bool _admin;
        private readonly bool _incoming;
        private readonly bool _outgoing;

        public MyAuthorizeImpl(bool admin, bool incoming, bool outgoing)
        {
            _admin = admin;
            _incoming = incoming;
            _outgoing = outgoing;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            ApplicationUser userInfo = filterContext.HttpContext.GetLoggedUser();

            if (userInfo == null)
            {
                filterContext.Result = new RedirectToActionResult("login", "account", new { @area = "" });
                return;
            }

            ApplicationDbContext dbContext = filterContext.HttpContext.RequestServices.GetService<ApplicationDbContext>();

            if (_admin && dbContext.Administrator.Any(i => i.AdministratorId == userInfo.Id))
            {
                await next();
                return;
            }

            if (_incoming && dbContext.Applicant.Any(i => i.ApplicantId == userInfo.Id))
            {
                await next();
                return;
            }

            if (_outgoing && dbContext.Applicant.Any(i => i.ApplicantId == userInfo.Id))
            {
                await next();
                return;
            }

            filterContext.Result = new RedirectToActionResult("login", "account", new { @area = "" });
        }
    }
}