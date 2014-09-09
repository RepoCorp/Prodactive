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
            List<LogEjercicio> lst=Manager.Instance.GetLogEjercicioByUserAndDates(User.Identity.Name, se.From, se.To);
            var a =lst.Select(x => new {fecha = x.FechaHora, pasos = x.Conteo}).ToList();
            return Json(new {a});
        }
    }

    public class Search
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}