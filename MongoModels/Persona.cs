using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MongoModels
{
    public class Persona
    {

        public Persona()
        { Cuentas= new Dictionary<string, string>();}

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        
        [Display(Name = "Tipo Persona")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        public string Nombre { get; set; }
        
        [Required]
        [Display(Name = "Apellidos")]
        [BsonIgnoreIfNull]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Identificacion")]
        public Int64 Identificacion { get; set; }

        
        [Required]
        [Display(Name = "Fecha De Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        [BsonIgnoreIfNull]
        public string Sexo { get; set; }
        
        [Display(Name = "Cuentas")]
        //[DataType(DataType.EmailAddress)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<string,string> Cuentas { get; set; }

        [Display(Name = "Peso")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.Double)]
        public Double Peso { get; set; }

        [Display(Name = "Estatura")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.Double)]
        public Double Estatura { get; set; }

    }
}