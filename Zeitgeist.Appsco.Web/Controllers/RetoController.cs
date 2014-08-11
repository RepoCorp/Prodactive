using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zeitgeist.Appsco.Web.App_Start;

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

        //
        // GET: /Reto/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /Reto/Create

        public ActionResult Create()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var result = Task.Factory.StartNew(() => { return manager.GetDivisiones(User.Identity.Name); });
            foreach (var a in result.Result)
                lst.Add(new SelectListItem() { Text = a.Name, Value = a.Id });
            SelectList sl = new SelectList(lst, "Value", "Text");
            ViewBag.Divisiones = sl;
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
            var query =manager.GetDivisiones(User.Identity.Name).Select(x=> new { Id=x.Id, Name= x.Name });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDeportes()
        {
            return Json(new {Name = "Caminar"}, JsonRequestBehavior.AllowGet);
        }
    }
}
