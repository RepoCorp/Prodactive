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

    public static class Logros
    {
        public static string PrimerParticipacionEnReto   = "1_participacion.png";
        public static string PrimerRetoGrupalGanado      = "1_reto_grupal_ganado.png";
        public static string PrimerRetoGrupalPerdido     = "1_reto_grupal_perdido.png";

        public static string PrimerRetoIndividualGanado  = "1_reto_individual_ganado.png";
        public static string PrimerRetoIndividualPerdido = "1_reto_individual_perdido.png";

        public static string ConstanciaMesX1 = "constancia_mes_x1.png";
        public static string ConstanciaMesX2 = "constancia_mes_x2.png";
        public static string ConstanciaMesX3 = "constancia_mes_x3.png";
        public static string ConstanciaSemX1 = "constancia_sem_x1.png";
        public static string ConstanciaSemX2 = "constancia_sem_x2.png";
        public static string ConstanciaSemX3 = "constancia_sem_x3.png";
        
        public static string LogroDiaX5 = "logro_dia_x5.png";
        public static string LogroDiaX10 = "logro_dia_x10.png";
        public static string LogroDiaX20 = "logro_dia_x20.png";
        public static string LogroMasConstante = "mas_constante.png";
        public static string LogroMasPuntosDiarios = "mas_puntos_diarios.png";
        public static string LogroMayorCrecimiento = "mayor_crecimiento.png";
        public static string LogroProdactivo       = "prodactivo.png";


        public static string RetoGanadoX5 = "retos_x5.png";
        public static string RetoGanadoX10 = "retos_x10.png";
        public static string RetoGanadoX20 = "retos_x20.png";
        public static string RetoGanadoX50 = "retos_x50.png";
        

    }
    /*
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
    */
    public class PersonaLogro
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
    }
}