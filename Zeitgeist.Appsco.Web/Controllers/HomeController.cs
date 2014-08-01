using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using MongoModels;
using Newtonsoft.Json;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Models;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private Manager manager = Manager.Instance;

        //[OutputCache(Duration = 60)]
        public ActionResult Index()
        {

            var reto = manager.GetReto();
            var estadisticas = manager.GetEstadisticasUsuarioReto(User.Identity.Name,reto.Id);
            var totalPasos=estadisticas.Sum(x => x.CantidadPasos);
            var totalReto=reto.Deportes["Caminar"];

            ViewBag.TotalPasos = totalPasos;
            ViewBag.Progreso = Math.Round((double)(totalPasos*100)/totalReto); 
            ViewBag.NumeroDias = estadisticas.Count();
            ViewBag.Reto = reto;
            ViewBag.FechaCierre = reto.FechaFin.ToString("yyyy-MM-dd");
            ViewBag.Estadisticas = estadisticas;
            //reto.Deportes.Where(x=>x)
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //Persona p = manager.GetDatosUsuario(User.Identity.Name);
            //manager.GetTotalRegistrosReto(User.Identity.Name);
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
            var     q = JsonConvert.DeserializeObject<ICollection<Registro>>(dataSave);
            Reto r=manager.GetReto();
            string id = User.Identity.Name;
            foreach (var a in q)
            {
                a.User = id;
                a.retoId = r.Id;
                if (!manager.SaveRegistro(a))
                    break;
            }

            return Json(new {state = true, message = "ok"});
            return View("Index");
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
