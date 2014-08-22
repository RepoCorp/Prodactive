using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.WebPages.Razor.Configuration;
using DotNetOpenAuth.AspNet.Clients;
using FluentMongo.Linq;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoModels;
using MongoProviders;
using ServiceStack.Common;
using WebMatrix.WebData;
using Zeitgeist.Appsco.Web.Api;
using Zeitgeist.Appsco.Web.Models;
using Zeitgeist.Appsco.Web.Properties;

namespace Zeitgeist.Appsco.Web.App_Start
{
    public class Manager
    {

        private static Manager _instance;
        private static object _mutex = new object();
        public MongoDatabase Database;

        public static Manager Instance
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
        
        private Manager()
        {
            
            var client = new MongoClient(Settings.Default.ConnectionString);
            var server = client.GetServer();
            Database   = server.GetDatabase(Settings.Default.Bd);
        }
        
        private MongoCollection<T> GetCollection<T>(string collection)
        {
            if (HttpRuntime.Cache[collection] == null)
            {
                var a = Database.GetCollection<T>(collection);
                insert(collection,a);
                return a;
            }
            return HttpRuntime.Cache[collection] as MongoCollection<T>;

        }

        private void insert(string key, object obj)
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
        public void RemoveCache(string key)
        {
            if (HttpRuntime.Cache[key] != null)
                HttpRuntime.Cache.Remove(key);
        }

        private bool Save<T>(T item,string collection)
        {
            var wc = GetCollection<T>(collection).Save(item);
            if (wc.Ok)
                return true;

            return false;
        }
        
        public bool SaveClient  (LandingData item)
        {
            return Save(item,Settings.Default.ClientCollection);
        }
       
        public bool SavePersona (Persona p)
        {
            return Save(p, Settings.Default.ColeccionPersona);
        }
        
        //public bool SaveRegistro(Registro registro)
        //{
        //    return Save(registro, Settings.Default.ColeccionRegistro);
        //}

        public Persona GetDatosUsuario(string user)
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
        
        internal Reto GetReto()
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }
        //internal List<Registro> GetEstadisticasUsuarioReto(string user,string reto)
        //{
        //    var wc =
        //        GetCollection<Registro>(Settings.Default.ColeccionRegistro)
        //            .AsQueryable()
        //            .Where(x => x.User == user && x.retoId == reto)
        //            .Select(x => x)
        //            .ToList();
        //    if (wc != null)
        //        return wc;
            
        //    return new List<Registro>();

        //}
        internal List<Liga> GetLigas(string p)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().Where(x => x.Entrenador == p).ToList();
        }
        internal bool SaveLiga(Liga liga)
        {
            return Save(liga, Settings.Default.ColeccionLiga);
        }

        public bool DeleteLiga(Liga liga)
        {
            WriteConcernResult wr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Remove(Query.EQ("_id",new ObjectId(liga.Id)));
            if (wr.Ok)
                return true;
            return false;
        }

        public Liga GetLigaById(string id)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().First(x => x.Id==id);
        }

        internal bool SaveInvitacion(Invitacion invitacion)
        {
            return Save(invitacion, Settings.Default.ColeccionInvitaciones);
        }

        public bool CorreoDisponible(string mail)
        {

           var a= GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", mail)).Count();
            if (a == 0)
                return true;
            return false;
        }

        public bool AddUserToleague(Invitacion invitacion)
        {
            /*
             * añadir persona a un liga, tengo el mail, pero necesito el usuario, 
             * tengo el usuario y lo guardo en la lista de usuarios pero no tengo el area
             */
            try
            {
                Task<Persona> tp=Task.Factory.StartNew(() => { return GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", invitacion.Mail)).First();});
                Liga    l = GetCollection<Liga>(Settings.Default.ColeccionLiga).Find(Query.EQ("_id", new ObjectId(invitacion.LigaId))).First();
                Persona p = tp.Result;
                var a     = p.Cuentas.Where(x => x.Value == invitacion.Mail).Select(x => x.Key).First();

                if(l.Usuarios==null)
                    l.Usuarios= new Dictionary<string, string>();

                l.Usuarios.Add(a, invitacion.Mail);
                WriteConcernResult wcr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Update(
                    Query.EQ("_id",new ObjectId(l.Id)),
                    Update.Replace(l));
                if(wcr.Ok)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddUserToleague(string idLiga,string user)
        {
            /*
             * añadir persona a un liga, tengo el mail, pero necesito el usuario, 
             * tengo el usuario y lo guardo en la lista de usuarios pero no tengo el area
             */
            try
            {
                Liga l = GetCollection<Liga>(Settings.Default.ColeccionLiga)
                        .Find(Query.EQ("_id", new ObjectId(idLiga)))
                        .First();

                Persona p = GetDatosUsuario(user);
                Task.Factory.StartNew(() =>
                {
                    return GetCollection<Invitacion>(Settings.Default.ColeccionInvitaciones)
                        .Update(Query.EQ("Cuentas.k", user), Update.Set("Estado", true));
                });
                

                if (l.Usuarios == null)
                    l.Usuarios = new Dictionary<string, string>();

                l.Usuarios.Add(user, p.Cuentas["user"]);
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

        public List<Division> GetDivisiones(string user)
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

        public List<Reto> GetRetos(string user)
        {
            List<Reto> l=GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Entrenador == user).ToList();
            if(l.Count==0)
                return new List<Reto>();
            return l;
        }

        internal Reto GetReto(string user)
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

        private List<Reto> GetUltimosReto()
        {
            var query =
                (from a in GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable()
                    orderby a.FechaFin descending
                    select a).Take(50);
            return query.ToList();
        }

        internal List<Reto> GetRetosActivos()
        {
            List<Reto> retos = GetCollection<Reto>(Settings.Default.ColeccionRetos).Find(Query.EQ("IsActivo", true)).ToList();
            if (retos.Count > 0)
                return retos;
            return new List<Reto>();
        }

        internal bool UpdateReto(Reto reto)
        {
            var wr=GetCollection<Reto>(Settings.Default.ColeccionRetos)
                .Update(Query.EQ("_id", new ObjectId(reto.Id)), Update.Replace(reto));
            if (wr.Ok)
            {
                RemoveCache(Settings.Default.ColeccionRetos);
                return true;
            }
            
            return false;
        }

        public bool SaveRegistroProgreso(RegistroProgreso reg)
        {
            return Save(reg, Settings.Default.ColeccionRegistroProgreso);
        }

        internal bool SaveReto(Reto r)
        {
            return Save(r, Settings.Default.ColeccionRetos);
        }

        internal Reto GetRetoById(string id)
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x=>x.Id==id).FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }

        public List<Liga> GetLeagueUserRegistered(string user)
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

        public List<Reto> GetRetosByIdLiga(string id)
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

        public List<LogEjercicio> GetDatosRetoEquipo(ICollection<string> equipos, string name,Reto reto)
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
        public List<LogEjercicio> GetDatosRetoEquipo(ICollection<string> equipos,Reto reto)
        {
            
            List<LogEjercicio> lst = new List<LogEjercicio>();
            var r = GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.In("_id",new BsonArray(equipos.Select(x => new ObjectId(x)).ToList()))).ToList();
            foreach (var e in r)
            {
                var l= GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new []
                                                                                            { Query.In("Usuario", new BsonArray(e.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                lst.AddRange(l);
            }
            return lst;
        }

        public List<LogEjercicio> GetLogEjercicioByIdReto(string id,string user)
        {
            Reto reto = GetRetoById(id);
            var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario",user),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
            return l;
        }
    }

   
}