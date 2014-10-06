using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Tips
    {
        public Tips()
        {
            LinkImage = "";
        }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Tipo { get; set; }

        public string Titulo { get; set; }

        public string Mensaje { get; set; }
        [BsonDefaultValue("")]
        public string LinkImage { get; set; }
    }

    public static class TipoTips
    {
        public static string Alimentacion = "Alimentación";
        public static string Deporte      = "Deporte";
        public static string Salud        = "Salud";
    }
}