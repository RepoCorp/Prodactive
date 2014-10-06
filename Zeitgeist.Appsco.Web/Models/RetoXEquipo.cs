using System.Collections.Generic;

namespace Zeitgeist.Appsco.Web.Models
{
    public class RetoXEquipo
    {

        public RetoXEquipo()
        {
            Detalles     = new List<DetalleRetosXEquipo>();
            TotalMejor   = 0;
        }

        public string   Equipo                      { get; set; }
        public int      PuntosTotales               { get; set; }
        public double   PorcentajePuntosTotales     { get; set; }
        public string   Mejor                       { get; set; }
        public int      TotalMejor                  { get; set; }
        public bool     MiEquipo                    { get; set; }
        public int      Posicion                    { get; set; }
        public List<DetalleRetosXEquipo> Detalles   { get; set; }

    }
}