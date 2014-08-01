using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Deporte
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nombre { get; set; }
        //public ICollection<string> Medidas { get; set; }//opcion multiple eje:pasos,velocidad,distancia
        public string Tips { get; set; }
        [BsonRepresentation(BsonType.String)]
        public ICollection<SensorType> Medidas { get; set; }
    }
}