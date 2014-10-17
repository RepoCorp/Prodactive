using MongoModels;
using Zeitgeist.Appsco.Web.Api;
using Zeitgeist.Appsco.Web.Api.Model;

namespace Zeitgeist.Appsco.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static LogEjercicio ToLogEjercicio(this RequestLogEjercicio req)
        {
            return new LogEjercicio()
            {
                Usuario   = req.Usuario,
                FechaHora = req.FechaHora,
                Ubicacion = req.Ubicacion,
                Conteo    = req.Conteo,
                Velocidad = req.Velocidad,
                Deporte = (string.IsNullOrEmpty(req.Deporte)?"Caminar":req.Deporte)
            };

        }

    }
}