using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
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

        [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetLigas()
        {
            List<Liga> ligas = manager.GetLeagueUserRegistered(User.Identity.Name);
            return Json(ligas.Select(x => new {id = x.Id, nombre = x.Nombre, entrenador = x.Entrenador}));
        }
        
        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetRetosByIdLiga(string id)
        {

            List<DetalleReto> lst= new List<DetalleReto>();
            List<Reto> retos = manager.GetRetosByIdLiga(id);
            //Puntos Individuales, Puntos Equipos, Puntos Lider o Meta
            
            foreach (var a in retos)
            {
                
                var l=manager.GetDatosRetoEquipo(a.Equipos,User.Identity.Name,a);
                if (l.Count() > 0)
                {
                    int totalEquipo        = l.Sum(x => x.Conteo);//datos Equipo
                    int totalPersona = l.Where(x => x.Usuario == User.Identity.Name).Sum(x => x.Conteo);//datos Personales
                    Int64 totalReto = 0;
                    if (a.Tipo == TipoReto.Superando)
                        manager.GetDatosRetoEquipo(a.Equipos, a);
                    else
                        totalReto = a.Meta;    
                    DetalleReto dr = new DetalleReto()
                    {
                        IdReto = a.Id,
                        Name = a.Name,
                        TotalEquipo = totalEquipo,
                        TotalReto = totalReto,
                        TotalUsuario = totalPersona,
                        //PorcentajeTotalEquipo = ((double)totalEquipo / (double)totalReto) * 100,
                        //PorcentajeTotalUsuario = ((double)totalPersona / (double)totalReto) * 100,
                        //PorcentajeTotalReto = 100
                    };
                    lst.Add(dr);
                }
               //datos equipo
            }
            return Json(lst);
        }
        
        [HttpPost]
        public JsonResult GetLogEjerciciosByIdReto(string id)
        {
            int i = 0;
            var lst = manager.GetLogEjercicioByIdReto(id, User.Identity.Name).Select(x => new { dia= i++, pasos= x.Conteo });
            return Json(lst);
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
}
