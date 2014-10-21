using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Hubs;

namespace Zeitgeist.Appsco.Web.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        private Manager manager = Manager.Instance;
        ConnectionMapping<string>  map= new ConnectionMapping<string>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ChatHub));

        public void Send(string usuario, string mensaje,string avatar,string liga)
        {
            Task.Factory.StartNew(() =>
            {
                ChatElement c = new ChatElement()
                {
                    User = usuario,
                    Message = mensaje,
                    Avatar = avatar,
                    Fecha = DateTime.Now,
                    Liga = liga
                };
                if (manager.SaveChat(c))
                {
                }
            });
            
            Clients.All.broadcastMessage(usuario, mensaje,avatar,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void Registro(string usuario, string liga)
        {
            string user       = Context.User.Identity.Name;
            string connection = Context.ConnectionId;
            map.Add(user,connection);

            //List<Equipo> lst = manager.GetEquiposByUser(usuario);
            //List<Reto> retos = manager.GetRetosActivosByLiga(liga);
            //log.Info(" "+lst.Count +" " +retos.Count );
            //Groups.Add()
            List<ChatElement> messages = manager.GetLastMessages(liga);
            if (messages.Count > 0)
            {
                messages = messages.OrderBy(x => x.Fecha).ToList();
                foreach (var elm in messages)
                {
                    Clients.Client(connection).broadcastMessage(elm.User, elm.Message, elm.Avatar, elm.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));        
                }
            }
            
            //Clients.All.broadcastMessage(usuario, "se ha registrado el usuario", "", DateTime.Now);
        }

        //logica del chat de retos
        public void RegistroChatReto(string usuario, string reto)
        {

            string user = Context.User.Identity.Name;
            string connection = Context.ConnectionId;
            map.Add(user, connection);

            //Cargar chat de reto.... crear grupos?
            /*
            List<ChatElement> messages = manager.GetLastMessages(liga);
            if (messages.Count > 0)
            {
                messages = messages.OrderBy(x => x.Fecha).ToList();
                foreach (var elm in messages)
                {
                    Clients.Client(connection).broadcastMessage(elm.User, elm.Message, elm.Avatar, elm.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }*/
        }

        public void SendChatReto(string usuario, string mensaje, string avatar, string reto)
        {
            /*
            Task.Factory.StartNew(() =>
            {
                ChatElement c = new ChatElement()
                {
                    User = usuario,
                    Message = mensaje,
                    Avatar = avatar,
                    Fecha = DateTime.Now,
                    Liga = liga
                };
                if (manager.SaveChat(c))
                {
                }
            });

            Clients.All.broadcastMessage(usuario, mensaje, avatar, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            */
        }
    }


     public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
