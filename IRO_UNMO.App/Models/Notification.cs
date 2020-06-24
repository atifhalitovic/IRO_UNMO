using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime DateTime { get; set; }
        public bool Seen { get; set; }
        public string Message { get; set; }

        public string URL { get; set; }

        public string UserToID { get; set; }
        public ApplicationUser UserTo { get; set; }

        public string UserFromID { get; set; }
        public ApplicationUser UserFrom { get; set; }
    }
}
