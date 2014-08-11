using System;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoModels;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System.Collections.Generic;
using Zeitgeist.Appsco.Web.App_Start;

namespace Zeitgeist.Appsco.Web.Api
{
    public class TestService : Service
    {

        public ResponsePeticion Any(Peticion peticion)
        {
            return new ResponsePeticion() {Response = "ok"};
        }

        public ResponseRecetas Any(Recetas receta)
        {
            Manager m=Manager.Instance;
            if (m.SaveReceta(receta))
                return new ResponseRecetas() {Message = "Se ha Guardado con exito", State = true};

            return new ResponseRecetas() { Message = "Error Al guardar", State = false };
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
    [Route("/recetas")]
    public class Recetas : IReturn<ResponseRecetas>
    {
        [BsonId]
        public string Name { get; set; }
        public List<Detalle> Detalles { get; set; }

    }

    public class Detalle
    {
        public string Message { get; set; }
        public int Value { get; set; }
    }

    public class ResponseRecetas : ResponseService
    {
        
    }

    public class ResponseService
    {
        public string Message { get; set; }
        public bool   State   { get; set; } 
    }


    public class Peticion
    {
        public string  A { get; set; }
    }
    public class ResponsePeticion : IReturn<Peticion>
    {
        public string Response { get; set; }
    }
    [Route("/login")]
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
    public class RegistroProgreso
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string   UserName { get; set; }
        public Int64    Pasos    { get; set; }
        public double   Calorias { get; set; }
        public DateTime Fecha    { get; set; }
        public string   IdReto   { get; set; }
    }
    public class ResponseRegistroProgreso: ResponseService,IReturn<RegistroProgreso>
    {

    }
}