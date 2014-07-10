using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeitgeist.Appsco.Web.Api
{
    public class TestService:Service
    {

        public ResponsePeticion Any(Peticion peticion)
        {
            return new ResponsePeticion() { Response = "ok" };
        }
    }

    public class Peticion
    {
        public string  A { get; set; }
    }
    public class ResponsePeticion : IReturn<Peticion>
    {
        public string Response { get; set; }

    }
}