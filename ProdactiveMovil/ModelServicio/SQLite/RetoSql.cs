using System;
using SQLite.Net.Attributes;

namespace ProdactiveMovil.ModelServicio.SQLite
{
    public class RetoSql
    {
        public RetoSql()
        {
            IsAcepted = false;
        }
        public RetoSql(Reto reto)
        {
            Id          = reto.Id;
            Division    = reto.Division;
            FechaInicio = reto.FechaInicio;
            FechaFin    = reto.FechaFin;
            Tipo        = reto.Tipo;
            IsActivo    = reto.IsActivo;
            Premio      = reto.Premio;
            
        }
        [PrimaryKey]
        public string Id { get; set; }
        public string Division { get; set; }
        public string Owner { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Tipo { get; set; }
        public bool IsActivo { get; set; }
        public string Premio { get; set; }
        public bool IsAcepted { get; set; }
    }
}