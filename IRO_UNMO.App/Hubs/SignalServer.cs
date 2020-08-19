using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using IRO_UNMO.Web.Helper;
using Microsoft.AspNetCore.Http;

namespace IRO_UNMO.App.Subscription
{
    //[Authorize]
    public class SignalServer : Hub
    {
        private readonly IMyUser _user;
        private readonly ApplicationDbContext  _db;
        private readonly INotification _notifikacijaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignalServer(IMyUser user, UserManager<ApplicationUser> userManager, ApplicationDbContext db, INotification notifikacija)
        {
            _notifikacijaService = notifikacija;
            _db = db;
            _userManager = userManager;
            _user = user;
        }
        
        public  Task getNotification(int brNoti)
        {
           return  _notifikacijaService.getAll(brNoti, Context.ConnectionId);
        } 
        
        public void deselectNotification(int id)
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
            var Userid = IRO_UNMO.Web.Helper.Autentifikacija.GetLoggedUser(Context.GetHttpContext()).Id;
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
