using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MongoModels
{

    public class Invitacion
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string LigaId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Mail   { get; set; }

        public bool   Estado { get; set; }
    }

    public class Liga
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id    { get; set; }
        public string Owner { get; set; }
        [Required]
        public string Name  { get; set; }
        [Required]
        [BsonDefaultValue("Freemium")]
        public string Plan  { get; set; }
        public ICollection<string> Divisiones { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<string, UsuarioLiga> Usuarios { get; set; }

    }

    public class UsuarioLiga
    {
        public string AreaId { get; set; }
        public bool   Estado { get; set; }
    }

    public class Area
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }

}