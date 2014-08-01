using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class Division
    {
        //[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id    { get; set; }

        public string LigaId { get; set; }
        public string Name  { get; set; }
        public ICollection<Equipo> Equipos { get; set; }

    }
}