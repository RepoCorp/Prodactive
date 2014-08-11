using System;
using System.Collections.Generic;

namespace ProdactiveMovil.ModelServicio
{
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
}