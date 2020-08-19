using IRO_UNMO.App.Data;
using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Subscription
{
    public class NotificationService : INotification
    {
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<SignalServer> _Hub;
        private readonly IMyUser _user;

        public NotificationService(ApplicationDbContext db, IHubContext<SignalServer> hub, IMyUser myUser)
        {
            _db = db;
            _Hub = hub;
            _user = myUser;
        }

        public void sendToApplicant(string to, string from, NotificationVM msg)
        {
            var applicant = _db.Applicant.Where(x => x.ApplicantId == to).Select(x => x.ApplicationUser).FirstOrDefault();
            var user = _user.getUserById(from.ToString()).Result;

            var vrijeme = DateTime.Now;
            //message.Picture = user.Picture;
            msg.User = "Administrator"; // user.Name + " " + user.Surname;
            msg.Seen = false;
            //msg.Time = vrijeme.ToString("hh:mm:ss");
            msg.Time = vrijeme.ToShortDateString();//("hh:mm:ss");
            var temp = new Notification()
            {
                Seen = msg.Seen,
                Message = msg.Message,
                URL = msg.Url,
                UserFromID = from,
                UserToID = applicant.Id,
                DateTime = vrijeme
            };
            _db.Notification.Add(temp);

            _db.SaveChanges();

            msg.NotificationId = temp.NotificationId;
            _Hub.Clients.Clients(applicant.SignalRToken).SendAsync("GetNotification", msg);
        }

        public void sendToAdmin(string to, string from, NotificationVM msg)
        {
            var admin = _db.Administrator.Where(x => x.AdministratorId == to).Select(x => x.ApplicationUser).FirstOrDefault();
            var applicant = _db.Applicant.Where(x => x.ApplicantId == from).Select(x => x.ApplicationUser).FirstOrDefault();

            var vrijeme = DateTime.Now;
            //message.Picture = user.Picture;
            msg.User = applicant.Name + " " + applicant.Surname;
            msg.Seen = false;
            msg.Time = vrijeme.ToString("hh:mm:ss");
            var temp = new Notification()
            {
                Seen = msg.Seen,
                Message = msg.Message,
                URL = msg.Url,
                UserToID = to,
                UserFromID = from,
                DateTime = vrijeme
            };
            _db.Notification.Add(temp);

            _db.SaveChanges();

            msg.NotificationId = temp.NotificationId;
            _Hub.Clients.Clients(admin.SignalRToken).SendAsync("GetNotification", msg);

        }

        public Task getAll(int br, string connectionId)
        {
            List<NotificationVM> notifikacije = _db.Notification.OrderBy(x => x.DateTime).Where(x => x.UserTo.SignalRToken == connectionId && x.Seen==false).Select(x => new NotificationVM()
            {
                NotificationId = x.NotificationId,
                Message = x.Message,
                //Picture = x.UserFrom.Slika,
                User = x.UserFrom.Name + " " + x.UserFrom.Surname,
                Time = x.DateTime.ToString("hh:mm"),
                Seen = x.Seen,
                Url = x.URL
            }).Take(br).ToList();
            return _Hub.Clients.Client(connectionId).SendAsync("getAll", notifikacije);
        }
    }
}
