using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoModels;
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reto/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
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
    }
}
