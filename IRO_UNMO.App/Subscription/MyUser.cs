using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Subscription
{
    public class MyUser : IMyUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public class NameUserIdProvider : IUserIdProvider
        {
            public string GetUserId(HubConnectionContext connection)
            {
                return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
            }
        }

        public MyUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser> getUser(ClaimsPrincipal claims)
        {
            ApplicationUser user = await _userManager.GetUserAsync(claims);
            return user;
        }
        public async Task<ApplicationUser> getUserById(string id)
        {
            ApplicationUser appUser = await _userManager.FindByIdAsync(id);
            return appUser;
        }
        public async Task<bool> updateUser(string id, string token)
        {
            try
            {
                ApplicationUser appUser = await _userManager.FindByNameAsync(id);
                appUser.SignalRToken = token;
                var x = await _userManager.UpdateAsync(appUser);
                if (x.Succeeded)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return false;
        }
    }
}

