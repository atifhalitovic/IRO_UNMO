using IRO_UNMO.App.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableDependency.SqlClient;
//using TableDependency.SqlClient.Base.Enums;
using TableDependency.Enums;
using System.IO;
using TableDependency.EventArgs;
using IRO_UNMO.App.Models;

namespace IRO_UNMO.App.Subscription
{
    public class NotificationDatabaseSubscription : IDatabaseSubscription
    {
        private bool disposedValue = false;
        private readonly IHubContext<NotificationHub> _hubContext;
        private SqlTableDependency<Application> _tableDependency;

        public NotificationDatabaseSubscription(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Configure(string connectionString)
        {
            _tableDependency = new SqlTableDependency<Application>(connectionString, null, null, null, null, DmlTriggerType.Update);
            _tableDependency.OnChanged += Changed;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Changed(object sender, RecordChangedEventArgs<Application> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                // TODO: manage the changed entity
                var changedEntity = e.Entity;
                _hubContext.Clients.All.SendAsync("updatestatus", changedEntity);
            }
        }

        #region IDisposable
        ~NotificationDatabaseSubscription()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tableDependency.Stop();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
