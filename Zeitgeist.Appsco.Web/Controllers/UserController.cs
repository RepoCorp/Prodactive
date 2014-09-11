using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoModels;
using Newtonsoft.Json;
using ServiceStack.Text;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Controllers
{
    public class UserController : Controller
    {
     
        
        [HttpPost]
        public JsonResult GetStatistics(string search)
        {
            Search se = JsonConvert.DeserializeObject<Search>(search);
            var lst   = Manager.Instance.GetLogEjercicioByUserAndDates(User.Identity.Name, se.From, se.To)
                .Select(x => new {Fecha = x.FechaHora.ToString("yyyy-MM-dd"), Pasos = x.Conteo, Deporte = x.Deporte})
                .OrderBy(x=>x.Fecha)
                .GroupBy(x => new {x.Fecha,x.Deporte})
                .Select(x => new { fecha = x.Key.Fecha, pasos = x.Sum(y=>y.Pasos), deporte = x.Key.Deporte });
            return Json(lst);
        }
    }

    public class Search
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}