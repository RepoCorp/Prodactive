using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    //ver 2
    public class Division
    {

        public Division()
        {
            Equipos = new Collection<string>();
        }

        //[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id    { get; set; }
        
        //public string LigaId { get; set; }
        
        public string Name  { get; set; }

        public string Descripcion { get; set; }

        public ICollection<string> Equipos { get; set; }

    }
}