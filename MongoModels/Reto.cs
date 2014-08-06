using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MongoModels
{
    public class Reto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Division { get; set; }
        public string Owner { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaInicio { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime FechaFin { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<string,int> Deportes { get; set; }
        public string Tipo   { get; set; }
        public bool IsActivo { get; set; }
        public string Premio { get; set; }

    }
}