using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.App.ViewModels;

namespace IRO_UNMO.App.Subscription
{
    public interface ILoggedUser
    {
        public LoggedUserVM GetLoggedUser(string id);
    }
}
