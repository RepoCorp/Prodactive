using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Equipo
    {

        public Equipo()
        {
            Miembros = new Collection<string>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string              Name { get; set; }
        public ICollection<string> Miembros { get; set; }
    }
}