using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize(Roles = "administrator")]
    public class ManageController : Controller
    {

        private readonly Manager manager = Manager.Instance;
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Panel()
        {
            string user = User.Identity.Name;
            return View();

        }

        public ActionResult CambiarRolesUsuario()
        {

            //manager.GetUsuarioLiga(User.Identity.Name);
            return View();
        }

        public ActionResult CreteDivision()
        {
            return View();
        }

        public ActionResult CreateDivision(Division d)
        {
            return View();
        }

        public ActionResult CreateLeague()
        {
            return View();
        }

        public ActionResult CreateLeague(Liga l)
        {
            if (ModelState.IsValid)
            {
                Persona p = manager.GetDatosUsuario(User.Identity.Name);

                l.Owner = p.Id;
                return RedirectToAction("Panel");
                //return RedirectoAction("Panel");    

            }
            
            return View(l);
        }

    }
}
