using System;
using System.Collections.Generic;
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
    public static class LogrosDiarios
    {
        public static string MasPuntosEnElDia = "Mas Puntos en el dia";
        public static string MayorCrecimiento = "Mayor crecimiento";
        public static string MasConstante     = "Más Constante";

        public static string LogroMasConstante = "mas_constante.png";
        public static string LogroMasPuntosDiarios = "mas_puntos_diarios.png";
        public static string LogroMayorCrecimiento = "mayor_crecimiento.png";

    }

    public static class Logros
    {
        public static string PrimerParticipacionEnReto = "1° Participacion En Reto";
        public static string PrimerRetoGrupalGanado = "1° Reto Grupal Ganado";
        public static string PrimerRetoGrupalPerdido = "1° Reto Grupal Perdido";

        public static string PrimerRetoIndividualGanado = "1° Reto individual ganado";
        public static string PrimerRetoIndividualPerdido = "1° Reto individual perdido";

        public static string ConstanciaMesX1 = "Constancia Mes X1";
        public static string ConstanciaMesX2 = "Constancia Mes X2";
        public static string ConstanciaMesX3 = "Constancia Mes X3";
        public static string ConstanciaSemX1 = "Constancia Sem X1";
        public static string ConstanciaSemX2 = "Constancia Sem X2";
        public static string ConstanciaSemX3 = "Constancia Sem X3";

        public static string LogroDiaX5 = "Logro Dia X5";
        public static string LogroDiaX10 = "Logro Dia X10";
        public static string LogroDiaX20 = "Logro Dia X20";
        public static string LogroProdactivo = "Logro Prodactivo";


        public static string RetoGanadoX5 = "Reto Ganado X5";
        public static string RetoGanadoX10 = "Reto Ganado X10";
        public static string RetoGanadoX20 = "Reto Ganado X20";
        public static string RetoGanadoX50 = "Reto Ganado X50";


        private static Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            
            {PrimerParticipacionEnReto   , "1_participacion.png"},
            {PrimerRetoGrupalGanado      , "1_reto_grupal_ganado.png"},
            {PrimerRetoGrupalPerdido     , "1_reto_grupal_perdido.png"},
            {PrimerRetoIndividualGanado  , "1_reto_individual_ganado.png"},
            {PrimerRetoIndividualPerdido , "1_reto_individual_perdido.png"},
            //{ConstanciaMesX1 , "constancia_mes_x1.png"},
            //{ConstanciaMesX2 , "constancia_mes_x2.png"},
            //{ConstanciaMesX3 , "constancia_mes_x3.png"},
            //{ConstanciaSemX1 , "constancia_sem_x1.png"},
            //{ConstanciaSemX2 , "constancia_sem_x2.png"},
            //{ConstanciaSemX3 , "constancia_sem_x3.png"},
            {LogroDiaX5 , "logro_dia_x5.png"},
            {LogroDiaX10 , "logro_dia_x10.png"},
            {LogroDiaX20 , "logro_dia_x20.png"},
            {LogroProdactivo , "prodactivo.png"},
            {RetoGanadoX5 , "retos_x5.png"},
            {RetoGanadoX10 , "retos_x10.png"},
            {RetoGanadoX20 , "retos_x20.png"},
            {RetoGanadoX50 , "retos_x50.png"},
        };



       
        public static Dictionary<string,string> GetLogros()
        {
            return dictionary;
        }

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