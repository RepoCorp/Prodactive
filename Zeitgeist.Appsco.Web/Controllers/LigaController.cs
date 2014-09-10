using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AjaxControlToolkit.HTMLEditor.Popups;
using MongoModels;
using ServiceStack.MiniProfiler;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Properties;
using ZeitGeist.Tools;

namespace Zeitgeist.Appsco.Web.Controllers
{
    //[Authorize]
    public class LigaController : Controller
    {
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AccountController));
        private readonly Manager manager = Manager.Instance;
        
        public ActionResult Index()
        {
            List<Liga> ls=manager.GetLigas(User.Identity.Name);
            return View(ls);
        }

        public ActionResult AceptarInvitacionLiga(string id)
        {
            ViewBag.IdLiga = id;
            Invitacion inv = new Invitacion() {LigaId = id, Estado = true, Mail = "prueba@123.com"};
            return View(inv);
        }

        [HttpPost]
        public ActionResult AceptarInvitacionLiga(Invitacion invitacion)
        {

            if (manager.AddUserToleague(invitacion.LigaId, User.Identity.Name))
            {
                return RedirectToAction("Index","Home");
            }
            

            ModelState.AddModelError("","Error al Añadir usuario a la liga");
            return View(invitacion);
        }

        // GET: /Liga/Create

        //public ActionResult Create()
        //{
        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    lst.Add(new SelectListItem() { Text = "Freemium", Value = "Freemium" });
        //    if (System.Web.Security.Roles.IsUserInRole("coach") || System.Web.Security.Roles.IsUserInRole("administrator"))
        //        lst.Add(new SelectListItem() { Text = "Standard", Value = "Standard" });
        //    SelectList sl = new SelectList(lst, "Value", "Text");
        //    ViewBag.Tipo = sl;

        //    return View();
        //}

        //
        // POST: /Liga/Create

        //[HttpPost]
        //public ActionResult Create(Liga liga )
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        liga.Entrenador = User.Identity.Name;
        //        if (manager.SaveLiga(liga))
        //            return RedirectToAction("Index");
                
        //        return View(liga);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("",ex);
        //        return View(liga);
        //    }
        //}

        //
        // GET: /Liga/Edit/5

        //public ActionResult Edit(string id)
        //{

        //    return View(manager.GetLigaById(id));
        //}

        ////
        //// POST: /Liga/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Liga/Delete/5

        //public ActionResult Delete(string id)
        //{
            
        //    return View(manager.GetLigaById(id));
        //}

        //
        // POST: /Liga/Delete/5

        //[HttpPost]
        //public ActionResult Delete(Liga liga)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        if (manager.DeleteLiga(liga))
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return View(liga);

        //    }
        //    catch(Exception ex)
        //    {
        //        ModelState.AddModelError("",ex);
        //        return View(liga);
        //    }
        //}


        public ActionResult EnvioInvitacion(string id)
        {
            Invitacion v = new Invitacion() {LigaId = id};
            return PartialView(v);
        }
        [HttpPost]
        public ActionResult EnvioInvitacion(Invitacion invitacion)
        {
            
            MailClass mc = new MailClass(Settings.Default.mail, Settings.Default.pass, "smtp.gmail.com", 587);
            invitacion.Remitente = User.Identity.Name;
            invitacion.LigaName  = manager.GetLigaById(invitacion.LigaId).Nombre;
            invitacion.Url       = "http://prodactive.co/Account/Login?ReturnUrl=%2fLiga%2fAceptarInvitacionLiga%2f" + invitacion.LigaId;
            if (!manager.CorreoDisponible(invitacion.Mail))
            {
                string body = RenderViewToString("Liga", "InvitacionLiga", invitacion);
                log.Info("Html invitacion");
                log.Info(body);
                //string message = "<b>soy una invitacion</b><a href=\"http://localhost:58640/Account/Login?ReturnUrl=%2fLiga%2fAceptarInvitacionLiga%2f" + invitacion.LigaId + "\">prodactive</a>";
                mc.Send(invitacion.Mail, "Te han invitado a pertenecer a una liga", body);
                TempData["MessageInvitacion"] = "El usuario ya existe en la plataforma, se ha enviado un correo de invitación para pertencer a la liga.";

            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (manager.SaveInvitacion(invitacion))
                    {
                        //enviar mail
                        string body = RenderViewToString( "Liga", "InvitacionLiga", invitacion);
                        log.Info("Html invitacion");
                        log.Info(body);
                
                        //string message = "<b>soy una invitacion</b><a href=\"http://localhost:58640/Account/Login?ReturnUrl=%2fLiga%2fAceptarInvitacionLiga%2f" + invitacion.LigaId + "\">prodactive</a>";
                        mc.Send(invitacion.Mail, "Invitacion Prodactive", body);
                        TempData["MessageInvitacion"] = "El usuario no existe en la plataforma, se enviado un correo de invitacion para ingresar en la plataforma.";
                       
                    }
                }
                else ModelState.AddModelError("", "no se pudo Guardar la invitacion");
            }

            return PartialView("_invitacion");
        }


        public ActionResult InvitacionProdactive()
        {
         
            return View();
        }


        public ActionResult InvitacionLiga()
        {
            
            ViewData["Remitente"] = "usuario x";
            ViewData["Liga"]      = "liga pruebas";
            ViewData["Url"]       = "www.prodactive.co";
            return View();
        }
        //public ActionResult Division()
        //{
        //    List<Division> list=manager.GetDivisiones(User.Identity.Name);
        //    return View(list);
        //}
        class FakeController : ControllerBase { protected override void ExecuteCore() { } }
        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                //var razorViewResult = razorViewEngine.FindView(controller.ControllerContext, viewName, "", false);
                //var viewContext = new ViewContext(controller.ControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                //fakeControllerContext.Controller.ViewData = (ViewDataDictionary) viewData;
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName,"", false);
                
                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
            return "";
        }
    }

    
}
