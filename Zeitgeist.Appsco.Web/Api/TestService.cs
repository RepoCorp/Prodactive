using System.Threading.Tasks;
using System.Web.Security;
using ServiceStack.ServiceInterface;
using Zeitgeist.Appsco.Web.Api.Model;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Api
{
    public class TestService : Service
    {
        //public ResponsePrueba Any(Prueba peticion)
        //{
        //    return new ResponsePrueba() {Fecha = DateTime.Now, Message = "", State = true};
        //}

        public ResponsePeticion Any(ServiceStatus serviceStatus)
        {
            return new ResponsePeticion() {Response = "ok"};
        }

        public LoginResponse Any(Model.Login login)
        {
            bool rt = Membership.ValidateUser(login.User, login.Pass);
            if (!rt)
            {
                return new LoginResponse()
                {
                    Message = "Verifique los Datos",
                    State = false
                };
            }
            Manager m = Manager.Instance;
            var t1= Task.Factory.StartNew(() =>
            {
                return m.GetDatosUsuario(login.User);
            });

            //var t2 =Task.Factory.StartNew(() => { return m.GetReto(login.User);});
            
            var w = t1.Result;
            //var r = t2.Result;
            
            return new LoginResponse()
            {

                Message = "OK",
                State   = true,
                User    = login.User,
                Persona = w
              //  Reto    = r
            };
        }

        public ResponseLogEjercicio Any(RequestLogEjercicio reg)
        {
            Manager m = Manager.Instance;

            ResponseLogEjercicio res = new ResponseLogEjercicio();
            if (m.SaveRegistroProgreso(reg))
            {
                res.State = true;
                res.Message = "Se ha guardado con exito el registro.";
            }
            else
            {
                res.State = false;
                res.Message = "Error al guardar el progreso.";
            }
            return res;

        }
    }
    
    //public class Detalle
    //{
    //    public string Message { get; set; }
    //    public int Value { get; set; }
    //}
    //[Route("/prueba")]
    //public class Prueba
    //{

    //}

    //public class ResponsePrueba : ResponseService, IReturn<Prueba>
    //{
    //    public DateTime Fecha { get; set; }
    //}
}