using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using IRO_UNMO.App.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Infrastructure
{
    //[Authorize]
    public class SignalServer : Hub
    {
        private readonly IMyUser _user;
        private readonly NameUserIdProvider mrka;
        private readonly ApplicationDbContext  _db;
        private readonly INotification _notifikacijaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignalServer(IMyUser user, UserManager<ApplicationUser> userManager, ApplicationDbContext db, INotification notifikacija)
        {
            _notifikacijaService = notifikacija;
            _userManager = userManager;
            _user = user;
            _db = db;
        }
        
        public  Task getNotification(int brNoti)
        {
           return  _notifikacijaService.getAll(brNoti, Context.ConnectionId);
        } 
        
        public  void deselectNotification(int id)
        {
            var n = _db.Notification.Where(x => x.NotificationId == id).FirstOrDefault();
            if (n!=null)
            {
                n.Seen = true;
                _db.SaveChanges();
            }
        }

        public override Task OnConnectedAsync()
         {
            var Userid = Context.UserIdentifier;
            //var UserId2 = mrka.GetUserId(connection);
            var ConnectionId = Context.ConnectionId;
            var x = _user.updateUser(Userid, ConnectionId).Result;

            if (x == true)
            {
                return base.OnConnectedAsync();
            }
            return null;
        }

    }
}
