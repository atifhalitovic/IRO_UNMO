using IRO_UNMO.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Subscription
{
    public interface IMyUser
    {
        Task<ApplicationUser> getUser(ClaimsPrincipal claims);
        Task<ApplicationUser> getUserById(string id);
        Task<bool> updateUser(string id, string token);
    }
}
