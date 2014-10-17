using ServiceStack.ServiceHost;

namespace Zeitgeist.Appsco.Web.Api.Model
{
    [Route("/status")]
    public class ServiceStatus
    {
        public string  Message { get; set; }
    }

    public class ResponsePeticion : IReturn<ServiceStatus>
    {
        public string Response { get; set; }
    }

    
}