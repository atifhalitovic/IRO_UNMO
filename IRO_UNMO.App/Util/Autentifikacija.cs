using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using Microsoft.EntityFrameworkCore;

namespace IRO_UNMO.Web.Helper
{
    public static class Autentifikacija
    {
        private const string loggedUser = "loggedUser";

        public static void SetLoggedUser(this HttpContext context, ApplicationUser userInfo, bool saveCookies = false)
        {
            ApplicationDbContext dbContext = context.RequestServices.GetService<ApplicationDbContext>();

            string oldToken = context.Request.GetCookieJson<string>(loggedUser);

            if (oldToken != null)
            {
                Token delete = dbContext.Token.FirstOrDefault(i => i.Value == oldToken);

                if (delete != null)
                {
                    dbContext.Token.Remove(delete);
                    dbContext.SaveChanges();
                }
            }

            if (userInfo != null)
            {
                string token = Guid.NewGuid().ToString();

                dbContext.Token.Add(new Token
                {
                    Value = token,
                    ApplicationUserId = userInfo.Id,
                    Created = DateTime.Now
                });

                dbContext.SaveChanges();
                context.Response.SetCookieJson(loggedUser, token);
            }
        }

        public static ApplicationUser GetLoggedUser(this HttpContext context)
        {
            ApplicationDbContext dbContext = context.RequestServices.GetService<ApplicationDbContext>();

            string token = context.Request.GetCookieJson<string>(loggedUser);

            if (token == null)
                return null;

            return dbContext.Token
                .Where(x => x.Value == token)
                .Select(s => s.ApplicationUser)
                .SingleOrDefault();
        }

        public static string GetTrenutniToken(this HttpContext context)
        {
            return context.Request.GetCookieJson<string>(loggedUser);
        }
    }
}