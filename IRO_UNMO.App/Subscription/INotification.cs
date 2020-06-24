using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Subscription
{
    public interface INotification
    {
        void sendToApplicant(string to, string from, NotificationVM message);
        void sendToAdmin(string to,  string from, NotificationVM message);
        Task getAll(int br, string connectionID);
    }

    public class NotificationVM
    {
        public string Message { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public string Picture { get; set; }
        public bool Seen { get;  set; }
        public string Url { get; internal set; }
        public int NotificationId { get; internal set; }
    }
}
