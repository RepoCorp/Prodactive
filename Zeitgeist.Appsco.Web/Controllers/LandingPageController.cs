using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Models;

namespace Zeitgeist.Appsco.Web.Controllers
{
    public class LandingPageController : Controller
    {
        //
        // GET: /LandingPage/

        private readonly Manager manager = Manager.Instance;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(LandingData id)
        {
            if (ModelState.IsValid)
            {
                if (manager.SaveClient(id))
                    return RedirectToAction("Message");

                return View("Index",id);
            }
            else
                return View("Index", id);
            
        }

        public ActionResult Message()
        {
            ViewBag.Message = "Se han Ingresado con exito tus Datos, te estaremos informando proximamente";
            return View();
        }

    }


}
