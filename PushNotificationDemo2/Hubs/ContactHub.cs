using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PushNotificationDemo2.Hubs
{
    public class ContactHub : Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ContactHub>();
            context.Clients.All.displayContact();
        }
    }
}