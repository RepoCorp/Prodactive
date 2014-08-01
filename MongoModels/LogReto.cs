using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class LogReto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Reto { get; set; }
        public string Deporte { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaHora { get; set; }
        public ICollection<DetalleLogReto> Detalles { get; set; }
    }

    public class Registro
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string retoId { get; set; }

        public string User { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Cantidad Pasos")]
        public int CantidadPasos { get; set; }
    }
}