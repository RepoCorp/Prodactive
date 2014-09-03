using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using FluentMongo.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoModels;
using Zeitgeist.Appsco.Web.Api;
using Zeitgeist.Appsco.Web.Helpers;
using Zeitgeist.Appsco.Web.Models;
using Zeitgeist.Appsco.Web.Properties;

namespace Zeitgeist.Appsco.Web.App_Start
{
    internal class Manager
    {

        internal static Manager _instance;
        internal static object _mutex = new object();
        internal MongoDatabase Database;
        internal static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Manager));
        internal static Manager Instance
        {
			get {
			
				if (_instance == null)
				{
					lock(_mutex)
					{
						if (_instance == null)
						{
							_instance= new Manager();
						}
					}
				}
				return _instance;
			}
            
        }
        
        internal Manager()
        {
            
            var client = new MongoClient(Settings.Default.ConnectionString);
            var server = client.GetServer();
            Database   = server.GetDatabase(Settings.Default.Bd);
        }
        
        internal MongoCollection<T> GetCollection<T>(string collection)
        {
            if (HttpRuntime.Cache[collection] == null)
            {
                var a = Database.GetCollection<T>(collection);
                insert(collection,a);
                return a;
            }
            return HttpRuntime.Cache[collection] as MongoCollection<T>;

        }

        internal void insert(string key, object obj)
        {
            HttpRuntime.Cache.Insert(
                /* key */ key,
                /* value */ obj,
                /* dependencies */ null,
                /* absoluteExpiration */ Cache.NoAbsoluteExpiration,
                /* slidingExpiration */ Cache.NoSlidingExpiration,
                /* priority */ CacheItemPriority.NotRemovable,
                /* onRemoveCallback */ null);
        }
        internal void RemoveCache(string key)
        {
            if (HttpRuntime.Cache[key] != null)
                HttpRuntime.Cache.Remove(key);
        }

        internal bool Save<T>             (T item,string collection)
        {
            var wc = GetCollection<T>(collection).Save(item);
            if (wc.Ok)
                return true;

            return false;
        }
        internal bool SaveClient          (LandingData item)
        {
            return Save(item,Settings.Default.ClientCollection);
        }
        internal bool SavePersona         (Persona p)
        {
            return Save(p, Settings.Default.ColeccionPersona);
        }
        internal bool SaveLiga            (Liga liga)
        {
            return Save(liga, Settings.Default.ColeccionLiga);
        }
        internal bool SaveEquipo          (Equipo equipo)
        {
            return Save(equipo, Settings.Default.CollectionEquipos);
        }
        internal bool SaveDivision        (Division division)
        {
            return Save(division, Settings.Default.ColeccionDivision);
        }
        internal bool SaveRegistroProgreso(RequestLogEjercicio reg)
        {
            return Save(reg.ToLogEjercicio(), Settings.Default.CollectionLogEjercicio);
        }
        internal bool SaveInvitacion      (Invitacion invitacion)
        {
            return Save(invitacion, Settings.Default.ColeccionInvitaciones);
        }
        internal bool SaveReto            (Reto r)
        {
            return Save(r, Settings.Default.ColeccionRetos);
        }
        internal bool UpdateReto          (Reto reto)
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos)
                .Update(Query.EQ("_id", new ObjectId(reto.Id)), Update.Replace(reto));
            if (wr.Ok)
            {
                RemoveCache(Settings.Default.ColeccionRetos);
                return true;
            }

            return false;
        }



        internal bool DeleteLiga(Liga liga)
        {
            WriteConcernResult wr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Remove(Query.EQ("_id", new ObjectId(liga.Id)));
            if (wr.Ok)
                return true;
            return false;
        }

        internal bool AddUserToleague(Invitacion invitacion)
        {
            try
            {
                
                Task<Persona> tp = Task.Factory.StartNew(() => { return GetDatosUsuarioByMail(invitacion.Mail); });
                Liga l = GetLigaById(invitacion.Id);
                
                Persona p = tp.Result;
                var a = p.Cuentas.Where(x => x.Value == invitacion.Mail).Select(x => x.Key).First();

                if (l.Usuarios == null)
                    l.Usuarios = new Dictionary<string, string>();

                l.Usuarios.Add(a, invitacion.Mail);
                WriteConcernResult wcr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Update(
                    Query.EQ("_id", new ObjectId(l.Id)),
                    Update.Replace(l));
                if (wcr.Ok)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal bool AddUserToleague(string idLiga, string user)
        {
            /*
             * añadir persona a un liga, tengo el mail, pero necesito el usuario, 
             * tengo el usuario y lo guardo en la lista de usuarios pero no tengo el area
             */
            try
            {

                Persona p = GetDatosUsuario(user);
                Task<Liga> t = Task.Factory.StartNew(() => { return GetLigaById(idLiga); });
                Task r = Task.Factory.StartNew(() =>
                {
                    return GetCollection<Invitacion>(Settings.Default.ColeccionInvitaciones)
                        .Update(Query.EQ("Cuentas.k", user), Update.Set("Estado", true));
                });

                Liga l = t.Result;
                if (l.Usuarios == null)
                    l.Usuarios = new Dictionary<string, string>();

                l.Usuarios.Add(user, p.Cuentas["user"]);
                WriteConcernResult wcr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Update(
                    Query.EQ("_id", new ObjectId(l.Id)),
                    Update.Replace(l));

                Task.WaitAll(new Task[] {r});
                if (wcr.Ok)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal bool CorreoDisponible(string mail)
        {

            var a = GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", mail)).Count();
            if (a == 0)
                return true;
            return false;
        }


        //internal bool SaveRegistro(Registro registro)
        //{
        //    return Save(registro, Settings.Default.ColeccionRegistro);
        //}

        internal Persona  GetDatosUsuario(string user)
        {
            Stopwatch sw= new Stopwatch();
            sw.Start();
            Persona p= new Persona();
            try
            {
                //var wr2 = GetCollection<Persona>(Settings.Default.ColeccionPersona).FindAll();
                var wr =GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.k", user));

                if (wr != null)
                    p= wr.First();
            }
            catch (Exception ex)
            {
            }
            sw.Stop();
            string s = sw.ElapsedMilliseconds.ToString();
            return p;

        }
        internal Persona  GetDatosUsuarioByMail(string mail)
        {
            Persona p = new Persona();
            try
            {
                //var wr2 = GetCollection<Persona>(Settings.Default.ColeccionPersona).FindAll();
                var wr = GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", mail));

                if (wr != null)
                    p = wr.First();
            }
            catch (Exception ex)
            {
                log.Error("Error al Buscar Datos de Usuario por Email",ex);
            }
            return p;
        }
        internal Reto     GetReto()
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }
        internal Liga     GetLigaById(string id)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().First(x => x.Id==id);
        }
        internal Division GetDivisionById(string division)
        {
            return GetCollection<Division>(Settings.Default.ColeccionDivision)
                .Find(Query.EQ("_id", new ObjectId(division)))
                .First();
        }
        internal Reto     GetRetoById(string id)
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Id == id).FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }
        internal Reto     GetReto(string user)
        {
            List<Reto> retos = GetRetosActivos();
            if (retos.Count == 0)
                retos = GetUltimosReto();
            foreach (var reto in retos)
            {
               List<Division> d= GetCollection<Division>(Settings.Default.ColeccionDivision).Find(Query.And(new[]
                {
                    Query.EQ("_id", new ObjectId(reto.Division)),
                    Query.EQ("Equipos.Miembros", user)
                })).ToList();
                if (d.Count>0)
                    return reto;
            }
            return new Reto();
        }

        internal      List<Liga>         GetLigas(string p)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().Where(x => x.Entrenador == p).ToList();
        }
        internal      List<Division>     GetDivisiones(string user)
        {
            List<Liga> lst = GetLigas(user).ToList();
            var res = lst.Select(x => x.Divisiones.ToList());
            int j = 0;
            //List<Division> divisiones=GetCollection<Division>(Settings.Default.ColeccionDivision)
            //    .AsQueryable()
            //    .Where(x => lst.Contains(y=> y.Division.co))
            //    .ToList();
            //if(divisiones.Count==0)


            return new List<Division>();

            //return divisiones;
        }
        internal      List<Reto>         GetRetos(string user)
        {
            List<Reto> l = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Entrenador == user).ToList();
            if (l.Count == 0)
                return new List<Reto>();
            return l;
        }
        internal      List<Reto>         GetRetosByLiga(string idLiga)
        {
            List<Reto> l = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Id == idLiga).ToList();
            if (l.Count == 0)
                return new List<Reto>();
            return l;
        }
        internal      List<Reto>         GetUltimosReto()
        {
            var query =
                (from a in GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable()
                    orderby a.FechaFin descending
                    select a).Take(50);
            return query.ToList();
        }
        internal      List<Reto>         GetRetosActivos()
        {
            List<Reto> retos = GetCollection<Reto>(Settings.Default.ColeccionRetos).Find(Query.EQ("IsActivo", true)).ToList();
            if (retos.Count > 0)
                return retos;
            return new List<Reto>();
        }
        internal      List<Liga>         GetLeagueUserRegistered(string user)
        {
            try
            {
                var r=GetCollection<Liga>(Settings.Default.ColeccionLiga).Find(Query.EQ("Usuarios.k", user));
                return r.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal      List<Reto>         GetRetosByIdLiga(string id)
        {
            try
            {
                return GetCollection<Reto>(Settings.Default.ColeccionRetos).Find(Query.EQ("Liga", id)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal      List<LogEjercicio> GetDatosRetoEquipo(ICollection<string> equipos, string name,Reto reto)
        {
                var r=GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.And(new []
                {
                    Query.In("_id",new BsonArray(equipos.Select(x => new ObjectId(x)).ToList())),
                    Query.EQ("Miembros", name)
                })).ToList();
                Equipo p= new Equipo();
                if (r.Count > 0)
                {
                    p = r.First();
                    return GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.In("Usuario", new BsonArray(p.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                }
            return new List<LogEjercicio>();
        }
        internal      List<LogEjercicio> GetDatosRetoEquipo(Reto reto)
        {
            List<LogEjercicio> lst = new List<LogEjercicio>();
            var r = GetEquipos(reto.Equipos);
            Parallel.ForEach(r, (e) =>
            {
                var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.In("Usuario", new BsonArray(e.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                lst.AddRange(l);
            });


            /*foreach (var e in r)
            {
                var l= GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new []
                                                                                            { Query.In("Usuario", new BsonArray(e.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                lst.AddRange(l);
            }*/
            return lst;
        }
        internal      List<LogEjercicio> GetLogEjercicioByIdReto(string id,string user)
        {
            Reto reto = GetRetoById(id);
            var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario",user),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
            return l;
        }
        internal      List<LogEjercicio> GetLogEjercicioByUserAndDates(string user, DateTime inicio, DateTime fin)
        {
            var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario",user),
                                                                                              Query.GTE("FechaHora", inicio),
                                                                                              Query.LTE("FechaHora", fin)
                                                                                             })).ToList();
            return l;
        }
        internal      List<Tips>         GetRandomTips()
        {
            return GetCollection<Tips>(Settings.Default.CollectionTips).AsQueryable().Take(6).ToList();
        }
        internal      List<Equipo>       GetEquipos(ICollection<string> equipos)
        {
            return GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.In("_id", new BsonArray(equipos.Select(x => new ObjectId(x)).ToList()))).ToList();
        }
        internal      List<Persona>      GetMiembrosEquipo(ICollection<string> miembros)
        {
            return GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.In("Cuentas.k",new BsonArray(miembros))).ToList();
        }
        internal      List<DetalleReto>  GetDetallesRetosByLiga(string id,string user)
        {
            List<DetalleReto> lst = new List<DetalleReto>();
            List<Reto> retos = GetRetosByIdLiga(id);
            //Puntos Individuales, Puntos Equipos, Puntos Lider o Meta

            foreach (var a in retos)
            {
                Task<List<Equipo>> t = Task.Factory.StartNew(() => GetEquipos(a.Equipos));

                List<LogEjercicio> datos = GetDatosRetoEquipo(a);
                List<Equipo> equipos = t.Result;
                int totalPersonal = 0;
                Equipo miEquipo = new Equipo();
                List<DetalleEquipo> team_detail = new List<DetalleEquipo>();
                if (datos.Count > 0)
                {
                    Parallel.ForEach(equipos, (equipo) =>
                    {
                        DetalleEquipo detall = new DetalleEquipo();
                        foreach (var miembro in equipo.Miembros)
                        {

                            //mis detalles personales
                            if (user == miembro)
                            {
                                detall.Total += totalPersonal = datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                                detall.MiEquipo = true;
                            }
                            else
                            {
                                detall.Total += datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                            }
                        }
                        detall.Equipo = equipo.Id;
                        team_detail.Add(detall);
                    });
                    int pos = 1;
                    foreach (var detalleEquipo in team_detail.OrderByDescending(x => x.Total))
                    {
                        detalleEquipo.Posicion = pos++;
                    }
                    if (a.Tipo == TipoReto.Superando)
                    {
                        DetalleReto dr = new DetalleReto()
                        {
                            IdReto = a.Id,
                            Name = a.Name,
                            TotalEquipo = team_detail.Where(x => x.MiEquipo).Sum(x => x.Total),
                            PosicionEquipo = team_detail.Where(x => x.MiEquipo).Select(x => x.Posicion).First(),
                            TotalReto = team_detail.Where(x => x.Posicion == 1).Sum(x => x.Total),
                            TotalUsuario = totalPersonal,
                        };
                        lst.Add(dr);
                    }
                    else
                    {
                        DetalleReto dr = new DetalleReto()
                        {
                            IdReto = a.Id,
                            Name = a.Name,
                            TotalEquipo = team_detail.Where(x => x.MiEquipo).Sum(x => x.Total),
                            PosicionEquipo = team_detail.Where(x => x.MiEquipo).Select(x => x.Posicion).First(),
                            TotalReto = a.Meta,
                            TotalUsuario = totalPersonal,
                        };
                        lst.Add(dr);
                    }
                }
            }
            return lst;
        }
    }

   
}