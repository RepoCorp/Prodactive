using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Manage;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize]
    [CompressContent]
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
        //[OutputCache(Duration = 600, VaryByCustom = "User", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [OutputCache(Duration = 600, VaryByCustom = "User", Location = OutputCacheLocation.Server)]
        public JsonResult GetLigas()
        {
            List<Liga> ligas = manager.GetLeagueUserRegistered(User.Identity.Name);
            var httpSessionStateBase = this.HttpContext.Session;
            if (httpSessionStateBase != null)
                httpSessionStateBase["IdLiga"] = ligas.First().Id;
            return Json(ligas.Select(x => new {id = x.Id, nombre = x.Nombre, entrenador = x.Entrenador, propia=(x.Entrenador==User.Identity.Name),invitacionesDisponibles=(x.UsuariosAdmitidosPlan-x.Usuarios.Count)}));
        }

        [HttpPost]
        [OutputCache(Duration = 600, VaryByCustom = "User", Location = OutputCacheLocation.Server)]
        public ActionResult GetUserData()
        {

           //List<Equipo> l=manager.GetEquiposByUser(User.Identity.Name);
           //l.Select(x => new {id = x.Id, equipo = x.Name});
           return Json(new { usuario = User.Identity.Name, avatar = "avatar2.png" });
        }

        [HttpPost]
        [OutputCache(Duration = 600, VaryByParam = "id",Location = OutputCacheLocation.Server)]
        public JsonResult GetRetosByIdLiga(string id)
        {
            List<Reto> retos = manager.GetRetosByIdLiga(id);
            return Json(retos);
        }
        
        [HttpPost]
        [OutputCache(Duration = 600, VaryByParam = "id", VaryByCustom = "User", Location = OutputCacheLocation.Server)]
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
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public JsonResult GetTips()
        {
            List<Tips> lst = manager.GetRandomTips();
            return Json(lst);
        }

        [HttpPost]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public JsonResult GetTipsDeporte()
        {
            List<Tips> lst = manager.GetTipsDeporte();
            return Json(lst);
        }

        [HttpPost]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public JsonResult GetTipsSalud()
        {
            List<Tips> lst = manager.GetTipsSalud();
            return Json(lst);
        }

        [HttpPost]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public JsonResult GetTipsAlimentacion()
        {
            List<Tips> lst = manager.GetTipsAlimentacion();
            return Json(lst);
        }

        [HttpPost]
        [OutputCache(Duration = 300, VaryByParam = "*", VaryByCustom = "User", Location = OutputCacheLocation.Server)]
        public JsonResult GetLogEjerciciosByUser()
        {
           var l = manager.GetLogEjercicioByUserAndDates(User.Identity.Name, DateTime.Now.AddDays(-5), DateTime.Now)
                .Select(x => new {Fecha = x.FechaHora.ToString("yyyy-MM-dd"), Pasos = x.Conteo, Deporte = x.Deporte})
                .OrderBy(x=>x.Fecha)
                .GroupBy(x => new {x.Fecha,x.Deporte})
                .Select(x => new { fecha = x.Key.Fecha, pasos = x.Sum(y=>y.Pasos), deporte = x.Key.Deporte });

            return Json(l);
        }

        /*
        public ActionResult Registro()
        {
            
            return View();
        }*/
        
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
        
    }


    
}
