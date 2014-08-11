using System;
using System.Collections.Generic;
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
            Deportes = new Dictionary<string, int>();
        }


        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Division { get; set; }
        
        public string Owner { get; set; }

        [Display(Name="Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaFin { get; set; }
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<string,int> Deportes { get; set; }
        
        public string Tipo   { get; set; }

        [Display(Name = "Reto Activo")]
        public bool IsActivo { get; set; }
        
        public string Premio { get; set; }

    }
}