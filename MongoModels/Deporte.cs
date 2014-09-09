using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{

    /// <summary>
    /// caminata,carrera,eliptica,ciclismo,patinaje,natación,Largatijas,Abdominales,SaltoCuerda
    /// </summary>
    /// 
    
    public class ChatElement
    {
         [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Liga { get; set; }
        public string User { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class Deporte
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string TipoConteo { get; set; }

        public int FactorConteo { get; set; }

        //public ICollection<string> Medidas { get; set; }//opcion multiple eje:pasos,velocidad,distancia
        public ICollection<string> Tips { get; set; }

        ////[BsonRepresentation(BsonType.String)]
        ////public ICollection<SensorType> Medidas { get; set; }
    }
    

    public static class TipoConteo
    {
        public static string Pasos      = "Pasos";
        public static string Metros     = "Metros";
        public static string Repeticion = "Repetición";
        public static string Saltos     = "Saltos";
    }
}