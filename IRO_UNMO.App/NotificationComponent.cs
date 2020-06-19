using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNetCore.SignalR;
using IRO_UNMO.App.Hubs;
using IRO_UNMO.App.Models;

namespace IRO_UNMO.App
{
    public class NotificationComponent
    {
        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = "lokalni";
            string sqlCommand = @"SELECT [ContactID],[ContactName],[ContactNo] from [dbo].[Contacts] where [AddedOn] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                //re-register notification
                RegisterNotification(DateTime.Now);

            }
        }

        public List<Contact> GetContacts(DateTime afterDate)
        {
            using (MyPushNotificationEntities dc = new MyPushNotificationEntities())
            {
                return dc.Contacts.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
            }
        }
    }
}