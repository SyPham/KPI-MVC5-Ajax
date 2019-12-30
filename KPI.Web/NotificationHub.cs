using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using KPI.Model.helpers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace KPI.Web
{
    public class NotificationHub : Hub
    {
        [HubMethodName("sendNotifications")]
        public static void SendNotifications()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.updateMessages();
        }
    }
}