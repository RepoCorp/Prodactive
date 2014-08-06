using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using MongoModels;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace console
{
    class Program
    {

        private static MongoDatabase Database;

        static void Main(string[] args)
        {

            string _connectionString = "mongodb://prodactive:pr0d4ct1v3@23.253.98.86:27017/prodactive";

            //string bd = _connectionString.Substring(_connectionString.LastIndexOf('/')+1, _connectionString.Length - (_connectionString.LastIndexOf('/') + 1));

            MongoClient mc = new MongoClient(_connectionString);
            var server = mc.GetServer();
            Database = server.GetDatabase("prodactive");
            Seed();
            Console.ReadLine();
            //Test();
        }

        private static void Test()
        {

            //miembros de los equipos de una liga
            //
            //var qqq = Database.GetCollection<Persona>("persona").AsQueryable().Where(x => x.Id == l.Owner).First();
            //Console.WriteLine(String.Format("Liga {0} - Owner {1} {2} ",l.Name,qqq.Nombre, qqq.Apellido));
            //Division query = Database.GetCollection<Division>("division").AsQueryable().Where(x => l.Divisiones.Contains(x.Id)).Select(x=>x).First();
            //Parallel.ForEach(query.Equipos, (b) =>
            //{
            //    Console.WriteLine("Equipo" + b.Name);
            //    foreach (var c in b.Miembros)
            //    {
            //        var qq = Database.GetCollection<Persona>("persona").AsQueryable().Where(x => x.Id == c).First();
            //        Console.WriteLine(String.Format("Id {0} \n Nombre {1} {2}", c.ToString(), qq.Nombre, qq.Apellido));
            //    }
            //});
            //saber en que divisiones estoy en este momento
            
            //string mail = "ddo88@hotmail.com";
            //Database.GetCollection<Persona>("persona").FindAll();
            //string[] values = new string[3];
            //Stopwatch s= new Stopwatch();
            //s.Start();
            //var p    = Database.GetCollection<Persona>("persona").AsQueryable().Where(x => x.Correos.Contains(mail)).First();
            //s.Stop();
            //values[0] = s.ElapsedMilliseconds.ToString();
            //s.Start();
            //var p2 = Database.GetCollection<Persona>("persona").Find(Query.EQ("Correos", mail)).First();
            //s.Stop();
            //values[1] = s.ElapsedMilliseconds.ToString();

            //s.Start();
            //var z    = Database.GetCollection<Division>("division").Find(Query.EQ("Equipos.Miembros", new ObjectId(p.Id))).First();
            //s.Stop();
            //values[2] = s.ElapsedMilliseconds.ToString();
            //int i = 0;
            //var div = Database.GetCollection<Division>("division").AsQueryable().Where(x=>x.GetMiembros(Database).Contains(p)).ToList();
            //var l = Database.GetCollection<Liga>("liga").FindOne();

            Console.ReadLine();
        }
        
        private static void Seed()
        {
            ////Persona p1 = new Persona()
            ////{
            ////    Nombre = "Daniel",
            ////    Apellido = "Osorio",
            ////    Correos = new Collection<string>() {"ddo88@hotmail.com"},
            ////    Identificacion = 1037587232,
            ////    Sexo = "M",
            ////    Type = "Natural"
            ////};
            ////Persona p2 = new Persona()
            ////{
            ////    Nombre = "Maria Clara",
            ////    Apellido = "De la cuesta",
            ////    Correos = new Collection<string>() {"juanita_dlc@hotmail.com"},
            ////    Identificacion = 1122334455,
            ////    Sexo = "F",
            ////    Type = "Natural"
            ////};

            ////Database.GetCollection<Persona>("persona").Save(p1);
            ////Database.GetCollection<Persona>("persona").Save(p2);

            //////necesito el id de los miembros
            /// 
            //var l= Database.GetCollection<Liga>("liga").FindOne();
            //Division d = new Division()
            //{
            //    LigaId = l.Id,
            //    Name = "Hombre Vs Mujeres",
            //    Equipos = new Collection<Equipo>()
            //    {
            //        new Equipo() {Name = "A", Miembros = new Collection<string>().ToArray()},
            //        new Equipo() {Name = "B", Miembros = new Collection<string>().ToArray()}
            //    }
            //};

            //Database.GetCollection<Division>("division").Save(d);
            //////necesito el id de la division
            //////necesito el id del owner
            //Liga l = new Liga()
            //{
            //    Name = "Piloto",
            //    Owner = "ddo88",
            //    Divisiones = new Collection<string>() { d.Id }
            //};

            //Database.GetCollection<Division>("liga").Save(l);

            //Deporte d1 = new Deporte()
            //{
            //    Nombre = "Caminar",
            //    Tips =
            //        "",
            //    Medidas = new Collection<SensorType>() { SensorType.WalkCounter }
            //};
            //Deporte d2 = new Deporte()
            //{
            //    Nombre = "Bicicleta",
            //    Tips =
            //        "",
            //    Medidas = new Collection<SensorType>() { SensorType.Distance, SensorType.Speed }
            //};
            //Deporte d3 = new Deporte()
            //{
            //    Nombre = "Patinar",
            //    Tips =
            //        "",
            //    Medidas = new Collection<SensorType>() { SensorType.Distance, SensorType.Speed }
            //};

            //Database.GetCollection<Deporte>("deporte").Save(d1);
            //Database.GetCollection<Deporte>("deporte").Save(d2);
            //Database.GetCollection<Deporte>("deporte").Save(d3);
            var a = Database.GetCollection<Division>("division").FindOne();
            var dic = new Dictionary<string, int>();
            dic.Add("Caminar",20000);
            Reto r1 = new Reto()
            {
                Owner    = "ddo88",
                Division = a.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(5),
                IsActivo = true,
                Deportes = dic
            };

            
            Database.GetCollection<Reto>("reto").Save(r1);
            //int i = 0;
        }
    }

   
}
