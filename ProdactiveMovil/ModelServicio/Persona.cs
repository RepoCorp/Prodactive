using System;
using System.Collections.Generic;

namespace ProdactiveMovil.ModelServicio
{
    public class Persona
    {

        public Persona()
        { Cuentas = new Dictionary<string, string>(); }

        public string Id { get; set; }

        public string Type { get; set; }

        public string Nombre { get; set; }
            
        public string Apellido { get; set; }

        public Int64 Identificacion { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; }
        //[DataType(DataType.EmailAddress)]
        public IDictionary<string, string> Cuentas { get; set; }

        public Double Peso { get; set; }

        public Double Estatura { get; set; }

    }
}