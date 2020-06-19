using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRO_UNMO.App.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string connectionString);
    }
}
