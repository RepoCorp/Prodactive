using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoModels
{
    public class LogEjercicio
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string   Id          { get; set; }
        public string   Usuario     { get; set; }
        public DateTime FechaHora   { get; set; }
        public string   Ubicacion   { get; set; }
        public string   Deporte     { get; set; }
        //este campo esta sujeto a cambios por ejemplo cambiarlo por un entero y manejar m/h 
        public double   Velocidad   { get; set; }
        public int      Conteo      { get; set; }
    }

    //se calcula por dia.
    public class LogLogrosDiarios
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string   Id          { get; set; }
        public string   IdReto      { get; set; }
        public DateTime Fecha       { get; set; }
        //atleta
        public string   Usuario     { get; set; }
        public string   LogroDiario { get; set; }
    }

    //Falta Añadirle Medallas, para todos los retos...
    public enum LogrosDiariosEnum
    {
        MasPuntosEnElDia,
        MayorCrecimiento,
        MasConstante
    }

    public enum Logros //medallita
    {
        PrimerParticipacionEnReto,
        PrimerRetoGrupalGanado,
        PrimerRetoGrupalPerdido,
        PrimerRetoIndividualGanado,
        PrimerRetoIndividualPerdido,
        RetosGanadosx5,
        RetosGanadosx10,
        RetosGanadosx20,
        RetosGanadosx50,
        Constanciax1Semana,//participar minimo 3 dias en una semana con un minimo de puntos
        Constanciax2Semana,//durante 2 semanas participa minimo 3 dias por semana con un minimo de puntos
        Constanciax1Mes,
        PrimerLogroDiariox1,
        LogroDiariox5,
        LogroDiariox10,
        LogroDiariox20
    }

    public class PersonaLogro
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
    }
}