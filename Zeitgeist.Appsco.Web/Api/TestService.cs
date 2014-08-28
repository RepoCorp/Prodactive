using System;
using System.Threading.Tasks;
using System.Web.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoModels;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Api
{
    public class TestService : Service
    {
        public ResponsePrueba Any(Prueba peticion)
        {
            return new ResponsePrueba() {Fecha = DateTime.Now, Message = "", State = true};
        }

        public ResponsePeticion Any(ServiceStatus serviceStatus)
        {
            return new ResponsePeticion() {Response = "ok"};
        }

        public LoginResponse Any(Login login)
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
    [Route("/prueba")]
    public class Prueba
    {

    }

    public class ResponsePrueba : ResponseService, IReturn<Prueba>
    {
        public DateTime Fecha { get; set; }
    }

    public class ResponseService
    {
        public string Message { get; set; }
        public bool   State   { get; set; } 
    }

    [Route("/status")]
    public class ServiceStatus
    {
        public string  Message { get; set; }
    }

    public class ResponsePeticion : IReturn<ServiceStatus>
    {
        public string Response { get; set; }
    }
    [Route("/login")]
    [Route("/login/{User}/{Pass}")]
    public class Login
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }

    public class LoginResponse : ResponseService, IReturn<Login>
    {
        public Persona Persona { get; set; }
        //public Reto    Reto    { get; set; }
        public string  User    { get; set; }
    }
    [Route("/LogEjercicio")]
    [Route("/LogEjercicio/{Usuario}/{FechaHora}/{Ubicacion}/{Conteo}/{Velocidad}")]
    public class RequestLogEjercicio
    {
        public string Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        public string Deporte { get; set; }
        //este campo esta sujeto a cambios por ejemplo cambiarlo por un entero y manejar m/h 
        public double Velocidad { get; set; }
        public int Conteo { get; set; }
        
    }
    public class ResponseLogEjercicio: ResponseService,IReturn<RequestLogEjercicio>
    {
        

    }
}