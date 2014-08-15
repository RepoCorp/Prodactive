using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Helpers;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize]
    public class RetoController : Controller
    {
        //
        // GET: /Reto/
        private readonly Manager manager = Manager.Instance;

        public ActionResult Index()
        {
            
            List<Reto> retos = manager.GetRetos(User.Identity.Name);
            return View(retos);
        }

        public ActionResult Create()
        {
            var result = manager.GetDivisiones(User.Identity.Name);
            ViewBag.Divisiones = Tools.GetSelectList<Division>(result, (x) => new ResultList { Name = x.Name, Value = x.Id }); ;
            return View();
        }
        //
        // POST: /Reto/Create

        [HttpPost]
        public ActionResult Create(string dataSave)
        {
            try
            {
                var reto      = JsonConvert.DeserializeObject<dynamic>(dataSave);
                Reto r        = new Reto();
                r.IsActivo    = reto.isActivo;
                r.FechaFin    = reto.fechaFin;
                r.FechaInicio = reto.fechaInicio;
                r.Division    = reto.division;
                r.Premio      = reto.premio;
                r.Tipo        = reto.tipo;
                r.Owner       = User.Identity.Name;
                string name   = reto.deportes[0].name;
                string valor  = reto.deportes[0].valor;

                r.Deportes.Add(name, Convert.ToInt32(valor));
                if (manager.SaveReto(r))
                {
                    return Json(new {Status = true, Message = "Has creado un nuevo reto."});
                }
            }
            catch(Exception ex)
            {
                
            }
            return Json(new { Status = true, Message = "error al guardar" });
        }

        //
        // GET: /Reto/Edit/5

        public ActionResult Edit(string id)
        {
            Reto r = manager.GetRetoById(id);
            return View(r);
        }

        //
        // POST: /Reto/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reto/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Reto/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult GetDivisiones()
        {
            var query = manager.GetDivisiones(User.Identity.Name).Select(x=> new { Id=x.Id, Name= x.Name });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDeportes()
        {
            return Json(new {Name = "Caminar"}, JsonRequestBehavior.AllowGet);
        }



        public class TipoReto
        {
            public string Name  { get; set; }
            public string Value { get; set; }
        }

        
        public ActionResult GetTipoReto()
        {

            List<TipoReto> list= new List<TipoReto>();
            list.Add(new TipoReto() { Name = "Superando", Value = "Superando" });
            list.Add(new TipoReto() { Name = "Primero En Cumplir", Value = "Primero" });
            return Json(new {
                
            },JsonRequestBehavior.AllowGet)
            ;
        }
    }
}
