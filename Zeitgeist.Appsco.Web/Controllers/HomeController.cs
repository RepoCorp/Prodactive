using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using AjaxControlToolkit;
using MongoModels;
using Newtonsoft.Json;
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

        //[OutputCache(Duration = 60)]
        public ActionResult Index()
        {

            var reto                 = manager.GetReto(User.Identity.Name);
            if (reto.Id == null)
            {
                ViewBag.RetoActivo   = false;
                ViewBag.Reto         = reto;
                ViewBag.FechaCierre  = DateTime.Now.AddDays(-2);   
            }
            else
            {
                ViewBag.RetoActivo   = reto.IsActivo;
                ViewBag.FechaCierre  = reto.FechaFin;
                ViewBag.Reto         = reto;

                var estadisticas     = manager.GetEstadisticasUsuarioReto(User.Identity.Name, reto.Id);
                var totalPasos       = estadisticas.Sum(x => x.CantidadPasos);
                var totalReto        = reto.Deportes["Caminar"];

                ViewBag.TotalPasos   = totalPasos;
                ViewBag.Progreso     = Math.Round((double)(totalPasos * 100) / totalReto);
                ViewBag.NumeroDias   = estadisticas.Count();
                ViewBag.Estadisticas = estadisticas;

                if (reto.IsActivo)
                    ViewBag.Message  = "Modify this template to jump-start your ASP.NET MVC application.";
                else
                    ViewBag.Message  = "El Reto ha finalizado.";
            }
            return View();
        }


        [OutputCache(Duration = 15)]
        public ActionResult GetStats()
        {
            
            //obetenr las estadisticas y redenrizar
            Estadisticas est = new Estadisticas()
            {
                Deporte = "Caminar",
                FinReto = DateTime.Now.AddDays(1),
                Recorrido = 120,
                Reto = "Caminar 200 kil"
            };
            return PartialView("Estadisticas", est);

        }

        public ActionResult Registro()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Registro(string dataSave)
        {
            var     q   = JsonConvert.DeserializeObject<ICollection<Registro>>(dataSave);
            Reto    r   = manager.GetReto();
            string  id  = User.Identity.Name;
            foreach (var a in q)
            {
                a.User      = id;
                a.retoId    = r.Id;
                if (!manager.SaveRegistro(a))
                    break;
            }

            return Json(new {state = true, message = "ok"});
        }

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
