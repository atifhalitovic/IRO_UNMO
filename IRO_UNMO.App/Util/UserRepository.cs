using IRO_UNMO.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Util
{
    public class UserRepository : IUserRepository
    
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void LogCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            service.LogAccessRequest(username);
        }
    }
}
