using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Equipo
    {
        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> Miembros { get; set; }
    }
}