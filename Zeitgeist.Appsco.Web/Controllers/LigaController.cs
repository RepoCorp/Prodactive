using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxControlToolkit.HTMLEditor.Popups;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using ZeitGeist.Tools;

namespace Zeitgeist.Appsco.Web.Controllers
{
    [Authorize]
    public class LigaController : Controller
    {
        //
        // GET: /Liga/
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

        public ActionResult Create()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Freemium", Value = "Freemium" });
            if (System.Web.Security.Roles.IsUserInRole("coach") || System.Web.Security.Roles.IsUserInRole("administrator"))
                lst.Add(new SelectListItem() { Text = "Standard", Value = "Standard" });
            SelectList sl = new SelectList(lst, "Value", "Text");
            ViewBag.Tipo = sl;

            return View();
        }

        //
        // POST: /Liga/Create

        [HttpPost]
        public ActionResult Create(Liga liga )
        {
            try
            {
                // TODO: Add insert logic here
                liga.Owner = User.Identity.Name;
                if (manager.SaveLiga(liga))
                    return RedirectToAction("Index");
                
                return View(liga);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex);
                return View(liga);
            }
        }

        //
        // GET: /Liga/Edit/5

        public ActionResult Edit(string id)
        {

            return View(manager.GetLigaById(id));
        }

        //
        // POST: /Liga/Edit/5

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
        // GET: /Liga/Delete/5

        public ActionResult Delete(string id)
        {
            
            return View(manager.GetLigaById(id));
        }

        //
        // POST: /Liga/Delete/5

        [HttpPost]
        public ActionResult Delete(Liga liga)
        {
            try
            {
                // TODO: Add delete logic here
                if (manager.DeleteLiga(liga))
                {
                    return RedirectToAction("Index");
                }
                return View(liga);

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex);
                return View(liga);
            }
        }


        public ActionResult EnviarInvitacion(string id)
        {
            Invitacion v = new Invitacion() {LigaId = id};
            return View(v);
        }
        [HttpPost]
        public ActionResult EnviarInvitacion(Invitacion invitacion)
        {
            MailClass mc = new MailClass("ddo88@hotmail.com", "@@hellsing01", "smtp.live.com", 587);
            
            if (!manager.CorreoDisponible(invitacion.Mail))
            {
                /*
                 * 
                 */
                string message = "<b>soy una invitacion</b><a href=\"http://localhost:58640/Account/Login?ReturnUrl=%2fLiga%2fAceptarInvitacionLiga%2f" + invitacion.LigaId + "\">prodactive</a>";
                mc.Send(invitacion.Mail, "Te han invitado a pertenecer a una liga", message);
                TempData["MessageInvitacion"] =
                    "El usuario ya existe en la plataforma, se ha enviado un correo de invitación para pertencer a la liga.";
                //if (manager.AddUserToleague(invitacion))
                //{
                //    return RedirectToAction("Index");
                //}
            }
            if (ModelState.IsValid)
            {
                if (manager.SaveInvitacion(invitacion))
                {
                    //enviar mail
                    string message = "<b>soy una invitacion</b><a href=\"http://localhost:58640/Account/Login?ReturnUrl=%2fLiga%2fAceptarInvitacionLiga%2f" + invitacion.LigaId + "\">prodactive</a>";
                    mc.Send(invitacion.Mail, "Invitacion Prodactive", message);
                    TempData["MessageInvitacion"] =
                        "El usuario no existe en la plataforma, se enviado un correo de invitacion para ingresar en la plataforma.";
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("","no se pudo Guardar la invitacion");
            return View(invitacion);
        }

        public ActionResult Division()
        {
            List<Division> list=manager.GetDivisiones(User.Identity.Name);
            return View(list);
        }
    }
}
