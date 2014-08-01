using System;
using System.Collections.Generic;
using System.Linq;
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
        public string User { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public double Peso { get; set; }
        public double Estatura { get; set; }

    }


    public class ResponseService
    {
        public string Message { get; set; }
        public bool State { get; set; }
    }

}