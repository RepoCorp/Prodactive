using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MongoModels
{
    
    //version 2
    public class Liga
    {

        public Liga()
        {
            Usuarios= new Dictionary<string, string>();
            Divisiones= new Collection<string>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id    { get; set; }
        
        public string Entrenador { get; set; }
        
        [Required]
        public string Nombre  { get; set; }
        
        [Required]
        [BsonDefaultValue("Freemium")]
        public string Plan  { get; set; }
        
        public ICollection<string> Divisiones { get; set; }

        //usuario y correo
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public IDictionary<string,string> Usuarios { get; set; }

        public int UsuariosAdmitidosPlan
        {
            get
            {
                if (Plan == "Freemium")
                    return 5;
                
                return 50;
            }
        }
    }

    //public class UsuarioLiga
    //{
    //    public string AreaId { get; set; }
    //    public bool   Estado { get; set; }
    //}

    //public class Area
    //{
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //}
    public static class Plan
    {
        public static string Estandar = "Estandar";
        public static string Freemium = "Freemium";
    }

    public class Invitacion
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string LigaId { get; set; }
       
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        [BsonIgnore]
        public string Remitente { get; set; }
        [BsonIgnore]
        public string Url { get; set; }
        [BsonIgnore]
        public string LigaName { get; set; }
        public bool Estado { get; set; }
    }

}