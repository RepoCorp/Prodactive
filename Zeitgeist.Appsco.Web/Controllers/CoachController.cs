using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize(Roles = "administrator, coach")]
    public class CoachController : Controller
    {
        //
        // GET: /Coach/

        public ActionResult Index()
        {
            return View();
        }

    }
}
