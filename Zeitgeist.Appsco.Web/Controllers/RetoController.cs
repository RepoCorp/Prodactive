using System.Collections.Generic;
using System.Linq;
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

        public JsonResult MaestroDetalle(string id)
        {

            List<RetoXEquipo> det    = new List<RetoXEquipo>();
            Reto              r      = manager.GetRetoById(id);

            Division            d    = manager.GetDivisionById(r.Division);
            //con este obtengo lo de todos los equipos...
            List<LogEjercicio> datos = manager.GetDatosRetoEquipo(r);
            

            List<Equipo> equipos = manager.GetEquipos(r.Equipos);
            foreach (var equipo in equipos)
            {
                RetoXEquipo de = new RetoXEquipo();
                de.Equipo      = equipo.Name;
                
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
