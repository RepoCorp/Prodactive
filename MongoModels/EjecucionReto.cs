using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class EjecucionReto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string       Id           { get; set; }
        public string       IdReto       { get; set; }
        public string       IdEquipo     { get; set; }
        //x dia
        public DateTime     Fecha        { get; set; }
        public int          Puntos       { get; set; }
        public MejorJugador MejorJugador { get; set; }
    }

    public class MejorJugador
    {
        public string User { get; set; }
        public int Puntos { get; set; }
    }
}