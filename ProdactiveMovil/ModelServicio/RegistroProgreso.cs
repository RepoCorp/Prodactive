using System;
using ServiceStack;
using SQLite.Net.Attributes;

namespace ProdactiveMovil.ModelServicio
{
    [Route("/RegistroProgreso")]
    public class RegistroProgreso
    {
        
        public string   Id       { get; set; }
        public string   UserName { get; set; }
        public Int64    Pasos    { get; set; }
        public double   Calorias { get; set; }
        public DateTime Fecha    { get; set; }
        public string   IdReto   { get; set; }
    }
    public class ResponseRegistroProgreso : ResponseService, IReturn<RegistroProgreso>
    {

    }
}