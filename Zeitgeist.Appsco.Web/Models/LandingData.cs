using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zeitgeist.Appsco.Web.Models
{
    [BsonDiscriminator("pos_client")]
    public class LandingData
    {
        [Required]
        [Display(Name = "E-Mail", Prompt = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        [BsonId]
        public string Email { get; set; }
    }
}