using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Tips
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Tipo { get; set; }

        public string Mensaje { get; set; }

        public string LinkImage { get; set; }
    }

    public static class TipoTips
    {
        public static string Alimentacion = "Alimentación";
        public static string Deporte      = "Deporte";
        public static string Salud        = "Salud";
    }
}