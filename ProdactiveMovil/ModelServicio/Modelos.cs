using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack;

namespace ProdactiveMovil.ModelServicio
{
    class Modelos
    {
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
        public class Persona
        {

            public Persona()
            { Cuentas = new Dictionary<string, string>(); }

            public string Id { get; set; }

            public string Type { get; set; }

            public string Nombre { get; set; }
            
            public string Apellido { get; set; }

            public Int64 Identificacion { get; set; }

            public DateTime FechaNacimiento { get; set; }

            public string Sexo { get; set; }
            //[DataType(DataType.EmailAddress)]
            public IDictionary<string, string> Cuentas { get; set; }

            public Double Peso { get; set; }

            public Double Estatura { get; set; }

        }

        public class Reto
        {
            public string Id { get; set; }
            public string Division { get; set; }
            public string Owner { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public IDictionary<string, int> Deportes { get; set; }
            public string Tipo { get; set; }
            public bool IsActivo { get; set; }
            public string Premio { get; set; }

        }

    public class ResponseService
    {
        public string Message { get; set; }
        public bool State { get; set; }
    }

}