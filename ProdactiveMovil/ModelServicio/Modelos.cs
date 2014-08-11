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
}