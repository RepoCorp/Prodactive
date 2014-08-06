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

        private static Orquestrator _instance;
        private static object _mutex = new object();

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
            Timer t= new Timer(new TimerCallback(VerificarRetos));
            t.Change(60*60*1000, 0);//Cada Hora

            Task.Factory.StartNew(() => VerificarRetos(new object()));
        }


        private void VerificarRetos(object o)
        {
            Task.Factory.StartNew(() => {
                Manager m = Manager.Instance;
                List<Reto> retos = m.GetRetosActivos();
                foreach (var reto in retos)
                {

                    if (reto.FechaFin < DateTime.Now)
                    {
                        reto.IsActivo = false;
                        m.UpdateReto(reto);
                    }
                }
            });
        }



    }
}