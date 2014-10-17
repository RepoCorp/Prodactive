using MongoModels;
using ServiceStack.ServiceHost;

namespace Zeitgeist.Appsco.Web.Api.Model
{
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
        public string User { get; set; }
    }

}