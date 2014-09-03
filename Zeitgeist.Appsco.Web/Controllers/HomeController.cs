using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

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
        
        [HttpPost]
        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 60)]
        public JsonResult GetLigas()
        {
            List<Liga> ligas = manager.GetLeagueUserRegistered(User.Identity.Name);
            var httpSessionStateBase = this.HttpContext.Session;
            if (httpSessionStateBase != null)
                httpSessionStateBase["IdLiga"] = ligas.First().Id;
            return Json(ligas.Select(x => new {id = x.Id, nombre = x.Nombre, entrenador = x.Entrenador, propia=(x.Entrenador==User.Identity.Name),invitacionesDisponibles=(x.UsuariosAdmitidosPlan-x.Usuarios.Count)}));
        }

        [HttpPost]
        public ActionResult GetUserData()
        {
            return Json(new { usuario = User.Identity.Name, avatar = "avatar2.png" });
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
            //Stopwatch sw= new Stopwatch();
            //sw.Start();
            List<DetalleReto> lst = manager.GetDetallesRetosByLiga(id,User.Identity.Name);
            GetEmptyReto(ref lst);
            //sw.Stop();
            //var s = sw.ElapsedMilliseconds.ToString();
            //log.Info("Tiempo de ejecucion de la tarea "+s);
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
        
        
       //estadistica basica del usuario
        [HttpPost]
        public JsonResult GetLogEjerciciosByUser()
        {
           var l = manager.GetLogEjercicioByUserAndDates(User.Identity.Name, DateTime.Now.AddDays(-5), DateTime.Now)
                .Select(x => new {Fecha = x.FechaHora.ToString("yyyy-MM-dd"), Pasos = x.Conteo, Deporte = x.Deporte})
                .OrderBy(x=>x.Fecha)
                .GroupBy(x => new {x.Fecha,x.Deporte})
                .Select(x => new { fecha = x.Key.Fecha, pasos = x.Sum(y=>y.Pasos), deporte = x.Key.Deporte });

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


    
}
