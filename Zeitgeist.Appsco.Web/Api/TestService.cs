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

            Manager m = Manager.Instance;
            var t1= Task.Factory.StartNew(() =>
            {
                return m.GetDatosUsuario(login.User);
            });

            var t2 =Task.Factory.StartNew(() => { return m.GetReto(login.User);});
            
            var w = t1.Result;
            var r = t2.Result;
            
            return new LoginResponse()
            {

                Message = "OK",
                State   = true,
                User    = login.User,
                Persona = w,
                Reto    = r
            };
        }

        public ResponseRegistroProgreso Any(RegistroProgreso reg)
        {
            Manager m = Manager.Instance;

            ResponseRegistroProgreso res= new ResponseRegistroProgreso();
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
        public Reto    Reto    { get; set; }
        public string  User    { get; set; }
    }
    [Route("/RegistroProgreso")]
    [Route("/RegistroProgreso/{UserName}/{Fecha}/{Pasos}/{Calorias}")]
    public class RegistroProgreso
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string   Id       { get; set; }
        public DateTime Fecha    { get; set; }
        public string   UserName { get; set; }
        public Int64    Pasos    { get; set; }
        public double   Calorias { get; set; }
        
    }
    public class ResponseRegistroProgreso: ResponseService,IReturn<RegistroProgreso>
    {

    }
}