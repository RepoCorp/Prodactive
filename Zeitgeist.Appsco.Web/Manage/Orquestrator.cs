using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Manage
{
    public class Orquestrator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Orquestrator));
        private static Orquestrator _instance;
        private static object       _mutex = new object();

        public static Orquestrator Instance
        {
			get {
			
				if (_instance == null)
				{
					lock(_mutex)
					{
						if (_instance == null)
						{
							_instance= new Orquestrator();
						}
					}
				}
				return _instance;
			}
            
        }
        
        private Orquestrator ()
        {
            Timer t= new Timer(VerificarRetos);
            t.Change(60*60*1000, 0);//Cada Hora

            Task.Factory.StartNew(() => VerificarRetos(new object()));
        }

        public void VerificarRetos(object o)
        {
            Task.Factory.StartNew(() => {
                Manager m = Manager.Instance;
                List<Reto> retos = m.GetRetosActivos();
                Parallel.ForEach(retos, (reto) =>
                {
            
                    if (reto.FechaFin < DateTime.Now)
                    {
                        reto.IsActivo = false;
                        m.UpdateReto(reto);
                        //tambien calcula el logro diario del ultimo dia
                        LogrosRetos(reto);
                    }
                    else
                    {
                        LogrosDiarios(reto);                        
                    }
                });
                
                
            });
        }

        public void LogrosDiarios(Reto reto)
        {
            DateTime fecha = DateTime.Now;
            if (fecha.Hour == 5) //cada dia a las 5 am, evaluamos el dia anterior
            {
                log.Info("Son las 5 am, inicio proceso de calculo de logros diarios");
                DateTime diaAnterior = fecha.AddDays(-1);
                if (reto.FechaInicio <= diaAnterior)//verifico si ya habia iniciado
                {
                    Task<List<Equipo>>  t       = Task.Factory.StartNew(() => Manager.Instance.GetEquipos(reto.Equipos));
                    List<LogEjercicio>  lst     = Manager.Instance.GetDatosRetoEquipo(reto);
                    List<Task> tareas= new List<Task>();
                    foreach (var equipo in t.Result)
                    {
                        tareas.Add(Task.Factory.StartNew(() => { CalcularPuntosDiarios    (reto, equipo, lst, diaAnterior); }));
                        tareas.Add(Task.Factory.StartNew(() => { CalcularMayorCrecimiento (reto, equipo, lst); }));
                    }

                    Task.WaitAll(tareas.ToArray());
                }
            }
        }


        private static void CalcularMayorCrecimiento(Reto reto, Equipo equipo, List<LogEjercicio> lst)
        {

            double   porcentajeMayor = 0;
            string   usuario         = "";
            DateTime diaAnterior     = DateTime.Now.AddDays(-1);
            int      diasReto        = (DateTime.Now.AddDays(-1) - reto.FechaInicio).Days;

            foreach (var miembro in equipo.Miembros)
            {
                int cantPasos    = 0;
                int cantPasosHoy = 0;
                for (int i = 0; i < diasReto; i++)
                {
                    cantPasos = lst.Where(x => x.FechaHora.Date == reto.FechaInicio.AddDays(i).Date &&  x.Usuario == miembro).Sum(x => x.Conteo);
                }
                cantPasosHoy = lst.Where(x => x.FechaHora.Date == DateTime.Now.AddDays(-1).Date && x.Usuario == miembro) .Sum(x => x.Conteo);
                double prom = (double)cantPasos/(double)diasReto;
                
                if (prom != 0)
                {
                    double porcentajeCrecimiento = ((double)cantPasosHoy * 100) / prom;
                    if (porcentajeCrecimiento > porcentajeMayor)
                    {
                        porcentajeMayor = porcentajeCrecimiento;
                        usuario = miembro;
                    }
                }
                
            }
            if (porcentajeMayor != 0)
            {
                LogLogrosDiarios ld = new LogLogrosDiarios();
                ld.Usuario          = usuario;
                ld.IdReto           = reto.Id;
                ld.LogroDiario      = MongoModels.LogrosDiarios.MayorCrecimiento;
                ld.Fecha = new DateTime(diaAnterior.Year, diaAnterior.Month, diaAnterior.Day, 0, 0, 0);
                try
                {
                    Manager.Instance.SaveLogLogrosDiarios(ld);
                }
                catch (Exception ex)
                {
                    log.Error("Ya existia el logro MayorCrecimiento en la bd", ex);
                }
                
            }

        }
        private static void CalcularPuntosDiarios(Reto reto,Equipo equipo, List<LogEjercicio> lst, DateTime diaAnterior)
        {
            DetalleEquipo de = new DetalleEquipo();
            de.IdEquipo = equipo.Id;
            de.Equipo = equipo.Name;

            foreach (var miembro in equipo.Miembros)
            {
                int totaldia = lst.Where(x => x.FechaHora.Date == diaAnterior.Date && x.Usuario == miembro).Sum(x => x.Conteo);
                if (de.Puntos < totaldia)
                {
                    de.Puntos = totaldia;
                    de.MejorMiembro = miembro;
                }
            }
            if (de.MejorMiembro != null)
            {
                LogLogrosDiarios ld = new LogLogrosDiarios();
                ld.Usuario          = de.MejorMiembro;
                ld.IdReto           = reto.Id;
                ld.LogroDiario      = MongoModels.LogrosDiarios.MasPuntosEnElDia;
                ld.Fecha            = new DateTime(diaAnterior.Year, diaAnterior.Month, diaAnterior.Day, 0, 0, 0);
                try
                {
                    Manager.Instance.SaveLogLogrosDiarios(ld);
                }
                catch (Exception ex)
                {
                    //ya existia el registro.
                    log.Error("Ya existia el logro PuntosDiarios en la bd", ex);
                }
                
            }
        }

        public class DetalleEquipo
        {
            public DetalleEquipo()
            {
                Puntos = 0;
            }

            public string IdEquipo { get; set; }
            public string Equipo { get; set; }


            public string MejorMiembro { get; set; }
            public int      Puntos { get; set; }
        }

        private void LogrosRetos(Reto reto)
        {
           /* Task<List<Equipo>>  t     = Task.Factory.StartNew(() =>  Manager.Instance.GetEquipos(reto.Equipos) );
            List<LogEjercicio>  lst   = Manager.Instance.GetDatosRetoEquipo(reto);
            List<Equipo>        teams = t.Result;

            DateTime fecha = reto.FechaInicio;
            while (fecha < reto.FechaFin){
                foreach (var equipo in teams)
                {
                    DetalleEquipo de= new DetalleEquipo();
                    de.IdEquipo = equipo.Id;
                    de.Equipo = equipo.Name;
                    
                    foreach (var miembro in equipo.Miembros)
                    {
                        int totadia=lst.Where(x => x.FechaHora.Date == fecha.Date && x.Usuario == miembro).Sum(x => x.Conteo);
                    }
                }


                fecha=fecha.AddDays(1);
            }*/

        }

    }
}