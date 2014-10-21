using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Zeitgeist.Appsco.Web.Models;

namespace Zeitgeist.Appsco.Web.Controllers
{

    
    [Authorize]
    [CompressContent]
    public class RetoController : Controller
    {
        //
        // GET: /Reto/
        private readonly Manager manager = Manager.Instance;

        /*
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
        */

        [HttpPost]
        [OutputCache(Duration = 300, VaryByCustom = "User",VaryByParam = "id", Location = OutputCacheLocation.Server)]
        public JsonResult MaestroDetalle(string id)
        {

            List<RetoXEquipo> det = new List<RetoXEquipo>();
            Reto r = manager.GetRetoById(id);
            var t1 = Task.Factory.StartNew(() =>
            {
                return manager.GetDivisionById(r.Division);
            });

            //con este obtengo lo de todos los equipos...
            var t2 = Task.Factory.StartNew(() =>
            {
                //return manager.GetDatosRetoEquipo(r);
                return manager.GetDatosRetoEquipo2(r);
            });
        
            List<Equipo> equipos = manager.GetEquipos(r.Equipos);
            List<LogDetalleUsuario> datos = t2.Result;
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
                    List<LogDetalleUsuario> detalleMiembro = datos.Where(x => x.Usuario == user).ToList(); // manager.GetLogEjercicioByIdReto(r.Id, user);

                    DetalleRetosXEquipo dre = new DetalleRetosXEquipo();
                    dre.Usuario = user;
                    de.PuntosTotales += dre.Total = detalleMiembro.Sum(x => x.Pasos);

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
                de.PorcentajePuntosTotales = ((double)de.PuntosTotales * 100) / (double)r.Meta;
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
            Division d = t1.Result;

            return Json(new { name = d.Name, descripcion = d.Descripcion, equipos = det });

        }

        [HttpPost]
        [OutputCache(Duration = 300, VaryByCustom = "User", VaryByParam = "id", Location = OutputCacheLocation.Server)]
        public JsonResult DetalleUsuario(string id)
        {
            Reto r= manager.GetRetoById(id);
            var lst = manager.GetDatosRetoUsuario2(r, User.Identity.Name);


            /*
            List<LogEjercicio> list = manager.GetDatosRetoUsuario(r, User.Identity.Name);
            Stopwatch sw= new Stopwatch();
            sw.Start();
            //var list2 = list.AsParallel().Select(x => new { fecha = x.FechaHora.ToString("yyyy-MM-dd"), conteo = x.Conteo }).GroupBy(x => x.fecha);
            //var list2 = list.Select(x => new { fecha = x.FechaHora.ToString("yyyy-MM-dd"), conteo = x.Conteo }).GroupBy(x => x.fecha);
            var list2 = list.Select(x => new { fecha =x.FechaHora, conteo = x.Conteo }).GroupBy(x => x.fecha);
            sw.Stop();
            int i = -1;

            List<LogDetalleUsuario> lst= new List<LogDetalleUsuario>();
            foreach (var b in list2)
            {
                LogDetalleUsuario o= new LogDetalleUsuario();
                o.FechaHora = b.Key;
                o.Pasos = b.ToList().AsParallel().Sum(x => x.conteo);
                var j = b.ToList().Sum(x => x.conteo);
                Console.WriteLine(j);
                lst.Add(o);
            }
            //list.Distinct(x=>x.)
            Console.WriteLine(i);

            */
            return Json(lst.Select(x=> new {x.Fecha,x.Pasos}));
        }
        
    }
}
