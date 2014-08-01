using System.Web.Security;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson.Serialization.Attributes;
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
            bool rt=
            Membership.ValidateUser(login.User, login.Pass);

            Manager m = Manager.Instance;
            var w=m.GetDatosUsuario(login.User);


            return new LoginResponse()
            {
                Message = "OK",
                State = true,
                User=login.User,
                Nombre = w.Nombre,
                Apellido = w.Apellido,
                Peso = w.Peso,
                Estatura = w.Estatura
            };
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
        public string User      { get; set; }
        public string Nombre    { get; set; }
        public string Apellido  { get; set; }
        public string Edad      { get; set; }
        public string Sexo      { get; set; }
        public double Peso      { get; set; }
        public double Estatura  { get; set; }
 
    }
}