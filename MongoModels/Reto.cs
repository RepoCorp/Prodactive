using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MongoModels
{
    public class Reto
    {
        public Reto()
        {
            Deportes = new List<string>();
            Equipos  = new List<string>();
        }


        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Liga { get; set; }

        public string Division   { get; set; }

        public string Owner { get; set; }
        //Owner
        public string Entrenador { get; set; }

        [Display(Name="Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaFin { get; set; }
        
        public ICollection<string> Deportes { get; set; }
        
        //[BsonDefaultValue("ElRetoEsConmigo")]
        public string Tipo   {get; set; }

        public Int64 Meta { get; set; }

        [Display(Name = "Reto Activo")]
        public bool IsActivo { get; set; }
        
        public string Premio { get; set; }

        public ICollection<string> Equipos { get; set; }

    }

    public static class TipoReto
    {
        public static string RetoPropio      = "El Reto es conmigo";
        public static string Superando       = "Superando Equipo Rival";
        public static string PrimeroEnLlegar = "Primero en Llegar";
        public static string Constancia      = "Constancia";
    }

}