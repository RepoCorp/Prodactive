﻿using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        private Manager manager = Manager.Instance;

        public void Send(string usuario, string mensaje,string avatar)
        {
            Clients.All.broadcastMessage(usuario, mensaje,avatar,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        //public void Registro(string usuario, string liga)
        //{

        //    Clients.All.broadcastMessage(usuario, "se ha registrado el usuario", "", DateTime.Now);
        //}
    }
}