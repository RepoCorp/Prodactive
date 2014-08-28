using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Manage;
using Zeitgeist.Appsco.Web.Models;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private Orquestrator orquestrator;

        protected override void Initialize(RequestContext requestContext)
        {
            if (orquestrator == null)
                orquestrator = Orquestrator.Instance;

            base.Initialize(requestContext);
        }

        private Manager manager = Manager.Instance;
        [HttpGet]
        //[OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult IndexP()
        //{
        //    return PartialView();
        //}

        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetLigas()
        {
            List<Liga> ligas = manager.GetLeagueUserRegistered(User.Identity.Name);

            var httpSessionStateBase = this.HttpContext.Session;
            if (httpSessionStateBase != null)
                httpSessionStateBase["IdLiga"] = ligas.First().Id;
            return Json(ligas.Select(x => new {id = x.Id, nombre = x.Nombre, entrenador = x.Entrenador}));
        }

        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetRetosByIdLiga(string id)
        {
            List<Reto> retos = manager.GetRetosByIdLiga(id);
            return Json(retos);
        }
        
        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetDetallesRetosByIdLiga(string id)
        {

            List<DetalleReto> lst= new List<DetalleReto>();
            List<Reto> retos = manager.GetRetosByIdLiga(id);
            //Puntos Individuales, Puntos Equipos, Puntos Lider o Meta
            
            foreach (var a in retos)
            {
                List<Equipo>        equipos = manager.GetEquipos(a.Equipos);
                List<LogEjercicio> datos    =  manager.GetDatosRetoEquipo(a);
                int totalPersonal = 0;
                Equipo miEquipo= new Equipo();
                List<DetalleEquipo> team_detail= new List<DetalleEquipo>();
                if (datos.Count > 0)
                {
                    foreach (var equipo in equipos)
                    {
                        DetalleEquipo detall= new DetalleEquipo();
                        foreach (var miembro in equipo.Miembros)
                        {
                            
                            //mis detalles personales
                            if (User.Identity.Name == miembro)
                            {
                                detall.Total    += totalPersonal= datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                                detall.MiEquipo  = true;
                            }
                            else
                            {
                                detall.Total += datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                            }
                        }
                        detall.Equipo = equipo.Id;
                        team_detail.Add(detall);
                    }
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
                            TotalReto = team_detail.Where(x=>x.Posicion==1).Sum(x=>x.Total),
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
            GetEmptyReto(ref lst);
            return Json(lst);
        }
        
        [HttpPost]
        public JsonResult GetTips()
        {
            List<Tips> lst = manager.GetRandomTips();
            return Json(lst);
        }


        private static void GetEmptyReto(ref List<DetalleReto> lst)
        {
            if (lst.Count == 0)
            {
                DetalleReto dr = new DetalleReto()
                {
                    IdReto = "",
                    Name = "Sin Retos",
                    TotalEquipo = 0,
                    TotalReto = 0,
                    TotalUsuario = 0,
                };
                lst.Add(dr);
            }
        }
        
        
       
        [HttpPost]
        public JsonResult GetLogEjerciciosByUser()
        {
            var l = manager.GetLogEjercicioByUserAndDates(User.Identity.Name, DateTime.Now.AddDays(-5), DateTime.Now)
                .Select(x => new {Fecha = x.FechaHora.ToString("yyyy-MM-dd"), Pasos = x.Conteo, Deporte = x.Deporte})
                .OrderBy(x=>x.Fecha)
                .GroupBy(x => new {x.Fecha,x.Deporte})
                .Select(x => new { fecha = x.Key.Fecha, pasos = x.Sum(y=>y.Pasos), deporte = x.Key.Deporte });


                //.Select(x => new {fecha = x.FechaHora.ToString("yyyy-MM-dd"), pasos = x.Conteo, deporte = x.Deporte})
                //.Select(y => new { fecha=y.fecha, pasos = y.pasos.Sum(),deporte=y.deporte});
                
            //var cont = l.Count();
            //var lst = manager.GetLogEjercicioByUserAndDates(User.Identity.Name, DateTime.Now.AddDays(-5), DateTime.Now).OrderBy(x => x.FechaHora).Select(x => new { fecha = x.FechaHora.ToString("yyyy-MM-dd"), pasos = x.Conteo , deporte = x.Deporte });
            //var lst = manager.GetLogEjercicioByIdReto(id, User.Identity.Name).OrderBy(x=>x.FechaHora).Select(x => new { dia= x.FechaHora.ToString("yyyy-MM-dd"), pasos= x.Conteo });
            return Json(l);
        }

        public ActionResult Registro()
        {
            
            return View();
        }

        //[HttpPost]
        //public ActionResult Registro(string dataSave)
        //{
        //    var     q   = JsonConvert.DeserializeObject<ICollection<Registro>>(dataSave);
        //    Reto    r   = manager.GetReto();
        //    string  id  = User.Identity.Name;
        //    foreach (var a in q)
        //    {
        //        a.User      = id;
        //        a.retoId    = r.Id;
        //        if (!manager.SaveRegistro(a))
        //            break;
        //    }

        //    return Json(new {state = true, message = "ok"});
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }


    public class DetalleEquipo
    {
        public string Equipo { get; set; }
        public int    Total    { get; set; }
        public int    Posicion { get; set; }
        public bool MiEquipo { get; set; }
    }
}
