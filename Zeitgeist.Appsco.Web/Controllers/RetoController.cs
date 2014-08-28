using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoModels;
using Newtonsoft.Json;
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


        public JsonResult MaestroDetalleReto(string id)
        {

            List<RetoXEquipo> det = new List<RetoXEquipo>();
            Reto r = manager.GetRetoById(id);

            Division d=manager.GetDivisionById(r.Division);
            //con este obtengo lo de todos los equipos...
            List<LogEjercicio> datos   = manager.GetDatosRetoEquipo(r);
            

            List<Equipo> equipos = manager.GetEquipos(r.Equipos);
            foreach (var equipo in equipos)
            {
                RetoXEquipo de = new RetoXEquipo();
                de.Equipo = equipo.Name;
                
                List<Persona> miembros = manager.GetMiembrosEquipo(equipo.Miembros);
                foreach (var miembro in miembros)
                {
                    string user = miembro.Cuentas.Keys.First();
                    if (user == User.Identity.Name)
                    {
                        de.MiEquipo = true;
                    }
                    List<LogEjercicio> detalleMiembro = datos.Where(x=>x.Usuario==user).ToList(); // manager.GetLogEjercicioByIdReto(r.Id, user);

                    DetalleRetosXEquipo dre= new DetalleRetosXEquipo();
                    dre.Usuario = user;
                    de.PuntosTotales += dre.Total = detalleMiembro.Sum(x => x.Conteo);
                    
                    if (dre.Total > de.TotalMejor)
                    {
                        de.Mejor = user;
                        de.TotalMejor = dre.Total;
                    }
                    de.Detalles.Add(dre);
                }
                int i = 1;
                foreach (var a in de.Detalles.OrderByDescending(x => x.Total))
                {
                    a.Posicion = i++;
                }

                de.Detalles = de.Detalles.OrderBy(x => x.Posicion).ToList();
                det.Add(de);
            }
            int pos = 1;
            foreach (var team in det.OrderByDescending(x => x.PuntosTotales))
            {
                team.Posicion = pos++;
                //if (!team.MiEquipo)
                //{
                //    team.Detalles= new List<DetalleRetosXEquipo>();
                //}
            }

            return Json(new { name = d.Name, descripcion=d.Descripcion , equipos = det });

        }

        
        //public ActionResult Create()
        //{
        //    var result = manager.GetDivisiones(User.Identity.Name);
        //    ViewBag.Divisiones = Tools.GetSelectList<Division>(result, (x) => new ResultList { Name = x.Name, Value = x.Id }); ;
        //    return View();
        //}
        ////
        //// POST: /Reto/Create

        //[HttpPost]
        //public ActionResult Create(string dataSave)
        //{
        //    try
        //    {
        //        var reto      = JsonConvert.DeserializeObject<dynamic>(dataSave);
        //        Reto r        = new Reto();
        //        r.IsActivo    = reto.isActivo;
        //        r.FechaFin    = reto.fechaFin;
        //        r.FechaInicio = reto.fechaInicio;
        //        r.Division    = reto.division;
        //        r.Premio      = reto.premio;
        //        r.Tipo        = reto.tipo;
        //        r.Entrenador  = User.Identity.Name;
        //        //string name   = reto.deportes[0].name;
        //        foreach (var name in reto.deportes)
        //        {
        //            r.Deportes.Add(name);
        //        }
        //        if (manager.SaveReto(r))
        //        {
        //            return Json(new {Status = true, Message = "Has creado un nuevo reto."});
        //        }
        //    }
        //    catch(Exception ex)
        //    {
                
        //    }
        //    return Json(new { Status = true, Message = "error al guardar" });
        //}

        ////
        //// GET: /Reto/Edit/5

        //public ActionResult Edit(string id)
        //{
        //    Reto r = manager.GetRetoById(id);
        //    return View(r);
        //}

        ////
        //// POST: /Reto/Edit/5

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

        ////
        //// GET: /Reto/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Reto/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //[HttpPost]
        //public ActionResult GetDivisiones()
        //{
        //    var query = manager.GetDivisiones(User.Identity.Name).Select(x=> new { Id=x.Id, Name= x.Name });
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult GetDeportes()
        //{
        //    return Json(new {Name = "Caminar"}, JsonRequestBehavior.AllowGet);
        //}



        //public class TipoReto
        //{
        //    public string Name  { get; set; }
        //    public string Value { get; set; }
        //}

        
        //public ActionResult GetTipoReto()
        //{

        //    List<TipoReto> list= new List<TipoReto>();
        //    list.Add(new TipoReto() { Name = "Superando", Value = "Superando" });
        //    list.Add(new TipoReto() { Name = "Primero En Cumplir", Value = "Primero" });
        //    return Json(new {
                
        //    },JsonRequestBehavior.AllowGet)
        //    ;
        //}
    }
    //nombrependiente de cambio
    public class RetoXEquipo
    {

        public RetoXEquipo()
        {
            Detalles = new List<DetalleRetosXEquipo>();
            TotalMejor = 0;
        }

        public string Equipo        { get; set; }
        public int    PuntosTotales { get; set; }
        public string Mejor         { get; set; }
        public int    TotalMejor    { get; set; }
        public bool   MiEquipo      { get; set; }
        public int    Posicion      { get; set; }
        public List<DetalleRetosXEquipo> Detalles { get; set; }

    }

    public class DetalleRetosXEquipo
    {
        public string Usuario   { get; set; }
        public int    Total     { get; set; }
        public int    Posicion  { get; set; }
    }
}
