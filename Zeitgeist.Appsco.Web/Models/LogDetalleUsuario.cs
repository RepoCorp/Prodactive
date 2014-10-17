using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Zeitgeist.Appsco.Web.Models
{
    public class LogDetalleUsuario
    {
        [BsonElement("FechaHora")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaHora { get; set; }
        
        [BsonElement("SumConteo")]
        public int Pasos { get; set; }

        [BsonElement("Usuario")]
        [BsonIgnoreIfNull]
        public string Usuario { get; set; }

        [BsonIgnore]
        public string Fecha {
            get
            {
                if (FechaHora != null)
                    return FechaHora.ToString("yyyy-MM-dd");
                return "";
            }
        }
    }

    //public class LogDetalleUsuario
    //{
    //    public string Fecha { get; set; }
    //    public int Pasos { get; set; }
    //}
}