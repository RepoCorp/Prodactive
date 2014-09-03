using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using MongoModels;

using MongoDB.Driver;
using ServiceStack;

using System.Threading;
namespace console
{
    public class PasoVel
    {
        public PasoVel(double Velocidad, int Conteo)
        {
            this.Conteo = Conteo;
            this.Velocidad = Velocidad;
        }

    
        public double Velocidad { get; set; }
        public int Conteo { get; set; }
    }
    public class MailClass
    {
        private readonly string _acount;
        private readonly string _pass;

        public MailClass(string acount, string pass)
        {
            _acount = acount;
            _pass = pass;
        }

        public bool Send(string destinatarios, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 60000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_acount, _pass);
            client.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            MailMessage mm = new MailMessage(_acount, destinatarios);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.Subject = subject;
            mm.Body = body;

            client.Send(mm);
            return true;
        }
    }
    class Program
    {

        private static MongoDatabase Database;

        static void Main(string[] args)
        {

            //var r = new JsvServiceClient("http://localhost:58640/api");
            //var rr = new RegistroProgreso()
            //{
            //    UserName = "ddo88",
            //    Fecha = DateTime.Now,
            //    IdReto = "",
            //    Calorias = 125,
            //    Pasos = 5000
            //};
            //try
            //{
            //    var resul = r.Send<ResponseRegistroProgreso>(rr);
            //}
            //catch (Exception ex)
            //{
            //    int i = 0;
            //}


            string _connectionString = "mongodb://prodactive:pr0d4ct1v3@23.253.98.86:27017/prodactive";

            //////string bd = _connectionString.Substring(_connectionString.LastIndexOf('/')+1, _connectionString.Length - (_connectionString.LastIndexOf('/') + 1));

            MongoClient mc = new MongoClient(_connectionString);
            var server = mc.GetServer();
            Database = server.GetDatabase("prodactive");

            //var result = Database.GetCollection<RegistroProgreso>("registro_progreso").AsQueryable().Where(x=>x.UserName=="ddo88").ToList().Sum(x=>x.Pasos);
                
            //int j = 0;
            //SeedDivisionLiga();
            //Seed();
            //SeedReto();

            //LogEjercicio le= new LogEjercicio();
            //le.Conteo = 150;
            //le.Deporte = "Caminar";
            //le.FechaHora = DateTime.Now;
            //le.Ubicacion = "lat= ,lon=";
            //le.Usuario   = "ddo88";
            //le.Velocidad = 4;
            //Database.GetCollection<LogEjercicio>("LogEjercicio").Save(le);
            //IngresoTips();
            
            Console.WriteLine("Todo Good");
            Console.ReadLine();
            ////Test();
        }

        private static void IngresoLogros()
        {
            
        }


        private static void IngresoTips()
        {
            Tips t= new Tips();
            t.Tipo = TipoTips.Alimentacion;
            t.Mensaje = "El mango y su vitamina A, aportan beneficios para nuestra piel, el cabello, la vista, los huesos y ademas, fortalece el sistema inmunologico. Asimismo por ser rica en vitamina C contribuye a la absorción de hierro y a la formación de glóbulos rojos.";
            Database.GetCollection<Tips>("Tips").Save(t);

            Tips t2 = new Tips();
            t2.Tipo = TipoTips.Alimentacion;
            t2.Mensaje = "Come cada vez que tengas hambre. Estudios revelan que las personas comen as de lo que su cuerpo requiere cuando se limitan a 1 o 2 comidas diariamente. Ademas no comer, durante largos periodos de tiempo, cansa al cerebro.";
            Database.GetCollection<Tips>("Tips").Save(t2);
                Tips t3 = new Tips();
                t3.Tipo = TipoTips.Deporte;
                t3.Mensaje = "Andar en bicicleta es una excelente opción para cuidar todo tu cuerpo y ademas para contribuir con el bienestar del planeta.";
                Database.GetCollection<Tips>("Tips").Save(t3);
                Tips t4 = new Tips();
                t4.Tipo = TipoTips.Salud;
                t4.Mensaje = "¡Hoy es el mejor día para caminar! Trae innumerables beneficios para tu salud, reducir el riesgo de padecer diabetes es uno de ellos.";
                Database.GetCollection<Tips>("Tips").Save(t4);

                Tips t5 = new Tips();
                t5.Tipo = TipoTips.Salud;
                t5.Mensaje = "El ejercicio libera endorfinas, unas sustancias capaces de crear sensación de relajación y felicidad.";
                Database.GetCollection<Tips>("Tips").Save(t5);
                Tips t6 = new Tips();
                t6.Tipo = TipoTips.Salud;
                t6.Mensaje = "Andar en bicicleta reduce el riesgo de un infarto en un 50%. Cuando pedaleas el ritmo cardíaco máximo aumenta  la presión arterial disminuye, es decir, el corazón trabaja economizando.";
                Database.GetCollection<Tips>("Tips").Save(t6);
            
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
            Dictionary<string,Queue<PasoVel>> dic = new Dictionary<string, Queue<PasoVel>>();

            List<PasoVel> paso= new List<PasoVel>(){new PasoVel(6110,	3900	),new PasoVel(4207,	4500	),new PasoVel(3706,	1600	),new PasoVel(7159,	6150	),new PasoVel(3776,	1280)};
            List<PasoVel> paso2= new List<PasoVel>(){new PasoVel(6598,	7440	),new PasoVel(5269,	8796	),new PasoVel(4328,	2869	),new PasoVel(7153,	10598	),new PasoVel(4121,	5986)};
            List<PasoVel> paso3= new List<PasoVel>(){new PasoVel(4258,	1596	),new PasoVel(3865,	2486	),new PasoVel(5698,	6257	),new PasoVel(4593,	5374	),new PasoVel(3886,	2047)};
            List<PasoVel> paso4= new List<PasoVel>(){new PasoVel(3658,	3526	),new PasoVel(4067,	4659	),new PasoVel(3749,	2759	),new PasoVel(3896,	3627	),new PasoVel(3869,	4685)};
            List<PasoVel> paso5= new List<PasoVel>(){new PasoVel(4956,	5326	),new PasoVel(4025,	2658	),new PasoVel(5036,	7598	),new PasoVel(5238,	6589	),new PasoVel(5987,	7259)};
            List<PasoVel> paso6= new List<PasoVel>(){new PasoVel(5025,	2158	),new PasoVel(4856,	1329	),new PasoVel(5039,	2698	),new PasoVel(6032,	6249	),new PasoVel(6124,	7526)};
            List<PasoVel> paso7= new List<PasoVel>(){new PasoVel(4569,	4368	),new PasoVel(5003,	5032	),new PasoVel(4695,	4268	),new PasoVel(5628,	7956	),new PasoVel(5133,	4025)};
            List<PasoVel> paso8= new List<PasoVel>(){new PasoVel(5248,	5369	),new PasoVel(5628,	7589	),new PasoVel(6035,	9148	),new PasoVel(6258,	11328	),new PasoVel(4865,	3596)};

            dic.Add("ddo88", new Queue<PasoVel>(paso));
            dic.Add("catalinadelacuesta", new Queue<PasoVel>(paso2));
            dic.Add("sthejoker", new Queue<PasoVel>(paso3));
            dic.Add("mmunera", new Queue<PasoVel>(paso4));
            dic.Add("jvanegas", new Queue<PasoVel>(paso5));
            dic.Add("mdelacue", new Queue<PasoVel>(paso6));
            dic.Add("laurablandon", new Queue<PasoVel>(paso7));
            dic.Add("restrepo.cafe", new Queue<PasoVel>(paso8));
            
            foreach (var usuario in dic)
            {
                for (int i = 0; i < 5; i++)
                {
                    LogEjercicio le= new LogEjercicio();
                    le.Usuario = usuario.Key;
                    le.FechaHora = DateTime.Now.AddDays(-i);
                    le.Deporte = "Caminar";
                    le.Ubicacion = "lat=, lon=";
                    PasoVel pv=usuario.Value.Dequeue();
                    le.Conteo = pv.Conteo;
                    le.Velocidad = pv.Velocidad;
                    Database.GetCollection<LogEjercicio>("LogEjercicio").Save(le);
                }
            }
            
        }

        private static void SeedReto()
        {

            LogEjercicio le= new LogEjercicio();
            

            Reto r = new Reto();
            r.Owner = "zeitgeist";
            r.Entrenador = "catalinadelacuesta";
            r.FechaInicio = new DateTime(2014, 8, 18);
            r.FechaFin = new DateTime(2014, 8, 22);
            r.Deportes.Add("53f54215f623e87a24777ec2");
            r.Tipo = TipoReto.PrimeroEnLlegar;
            r.Division = "53f54144f623e879c4e0cd85";
            r.Meta = 100000;
            r.IsActivo = true;
            r.Premio = "Panceroti para el desayuno del lunes";
            r.Equipos.Add("53f54144f623e879c4e0cd83");
            r.Equipos.Add("53f54144f623e879c4e0cd84");

            Database.GetCollection("reto").Save(r);
            Reto r2 = new Reto();
            r2.Owner = "zeitgeist";
            r2.Entrenador = "catalinadelacuesta";
            r2.FechaInicio = new DateTime(2014, 8, 25);
            r2.FechaFin = new DateTime(2014, 8, 29);
            r2.Deportes.Add("53f54215f623e87a24777ec2");
            r2.Tipo = TipoReto.Superando;
            r2.Division = "53f54145f623e879c4e0cd8a";
            r2.IsActivo = false;
            r2.Premio = "El dia viernes el ganador sale 30 minutos más temprano.";
            r2.Equipos.Add("53f54145f623e879c4e0cd88");
            r2.Equipos.Add("53f54145f623e879c4e0cd89");
            Database.GetCollection("reto").Save(r2);


            Reto r3 = new Reto();
            r3.Owner = "catalinadelacuesta";
            r3.Entrenador = "catalinadelacuesta";
            r3.FechaInicio = new DateTime(2014, 8, 18);
            r3.FechaFin = new DateTime(2014, 8, 29);
            r3.Deportes.Add("53f54215f623e87a24777ec2");
            r3.Tipo = TipoReto.Constancia;
            r3.Division = "53f54140f623e879c4e0cd72";
            r3.IsActivo = true;
            r.Meta = 6000;
            r3.Premio = "Botella de Boons.";
            r3.Equipos.Add("53f5413ef623e879c4e0cd70");
            r3.Equipos.Add("53f54140f623e879c4e0cd71");
            Database.GetCollection("reto").Save(r3);
            

        }

        private static void SeedDeportes()
        {
            Deporte a      = new Deporte();
            a.Nombre       = "Caminar";
            a.TipoConteo   = TipoConteo.Pasos;
            a.FactorConteo = 1;

            Deporte b= new Deporte();
            b.Nombre = "Ciclismo";
            b.TipoConteo = TipoConteo.Metros;
            b.FactorConteo = 1;

            Deporte c= new Deporte();
            c.Nombre = "Correr";
            c.TipoConteo = TipoConteo.Pasos;
            c.FactorConteo = 2;

            Deporte d= new Deporte();
            d.Nombre = "Patinaje";
            d.TipoConteo = TipoConteo.Pasos;
            d.FactorConteo = 1;
            
            Deporte e= new Deporte();
            e.Nombre = "Natación";
            e.TipoConteo = TipoConteo.Metros;
            e.FactorConteo = 2;

            Deporte f= new Deporte();
            f.Nombre = "Abdominales";
            f.TipoConteo = TipoConteo.Repeticion;
            f.FactorConteo = 3;

            Deporte g= new Deporte();
            g.Nombre = "Eliptica";
            g.TipoConteo = TipoConteo.Pasos;
            g.FactorConteo = 2;

            Deporte h= new Deporte();
            h.Nombre = "Velitas";
            h.TipoConteo = TipoConteo.Repeticion;
            h.FactorConteo = 3;

            Deporte i      = new Deporte();
            i.Nombre       = "Salto Cuerda";
            i.TipoConteo   = TipoConteo.Saltos;
            i.FactorConteo = 2;

            Database.GetCollection<Equipo>("deporte").Save(a);
            Database.GetCollection<Equipo>("deporte").Save(b);
            Database.GetCollection<Equipo>("deporte").Save(c);
            Database.GetCollection<Equipo>("deporte").Save(d);
            Database.GetCollection<Equipo>("deporte").Save(e);
            Database.GetCollection<Equipo>("deporte").Save(f);
            Database.GetCollection<Equipo>("deporte").Save(g);
            Database.GetCollection<Equipo>("deporte").Save(h);
            Database.GetCollection<Equipo>("deporte").Save(i);
        }

        private static void SeedDivisionLiga()
        {
            /* ---CREACION LIGAS--*/
            Liga l1 = new Liga();
            l1.Nombre = l1.Entrenador = "catalinadelacuesta";
            l1.Plan = "Freemium";
            l1.Usuarios.Add("catalinadelacuesta", "delacuesta@hotmail.com");
            l1.Usuarios.Add("mdelacue", "mdelacue@gmail.com");

            int i = 0;
            
            Equipo e1= new Equipo();
            e1.Name = i.ToString();
            e1.Miembros.Add("catalinadelacuesta");
            i++;
            Database.GetCollection<Equipo>("equipo").Save(e1);

            Equipo e2 = new Equipo();
            e2.Name = i.ToString();
            e2.Miembros.Add("mdelacue");
            Database.GetCollection<Equipo>("equipo").Save(e2);

            Division d1= new Division();
            d1.Name = "Individual";
            d1.Equipos.Add(e1.Id);
            d1.Equipos.Add(e2.Id);
            Database.GetCollection<Equipo>("division").Save(d1);

            l1.Divisiones.Add(d1.Id);
            Database.GetCollection<Equipo>("liga").Save(l1);
            // fin liga catalina

            Liga l3 = new Liga();
            l3.Nombre = l3.Entrenador = "sthejoker";
            l3.Plan = "Freemium";
            l3.Usuarios.Add("sthejoker", "sthejoker@gmail.com");

            i = 0;
            Equipo e3 = new Equipo();
            e3.Name = i.ToString();
            e3.Miembros.Add("sthejoker");
            Database.GetCollection<Equipo>("equipo").Save(e3);

            Division d2 = new Division();
            d2.Name = "Individual";
            d2.Equipos.Add(e3.Id);
            Database.GetCollection<Equipo>("division").Save(d2);
            
            l3.Divisiones.Add(d2.Id);
            Database.GetCollection<Equipo>("liga").Save(l3);

            // ------fin liga steven ----
            
            Liga l4 = new Liga();
            l4.Nombre = l4.Entrenador = "mdelacue";
            l4.Plan = "Freemium";
            l4.Usuarios.Add("mdelacue", "mdelacue@gmail.com");

            i = 0;
            Equipo e4 = new Equipo();
            e4.Name = i.ToString();
            e4.Miembros.Add("mdelacue");
            Database.GetCollection<Equipo>("equipo").Save(e4);

            Division d3 = new Division();
            d3.Name = "Individual";
            d3.Equipos.Add(e4.Id);
            Database.GetCollection<Equipo>("division").Save(d3);

            l4.Divisiones.Add(d3.Id);
            Database.GetCollection<Equipo>("liga").Save(l4);
            // --- fin liga mdelacue

            Liga l5 = new Liga();
            l5.Nombre = l5.Entrenador = "mmunera";
            l5.Plan = "Freemium";
            l5.Usuarios.Add("mmunera", "mmunera@importacionesintegrales.com");


            i = 0;
            Equipo e5 = new Equipo();
            e5.Name = i.ToString();
            e5.Miembros.Add("mmunera");
            Database.GetCollection<Equipo>("equipo").Save(e5);

            Division d4 = new Division();
            d4.Name = "Individual";
            d4.Equipos.Add(e5.Id);
            Database.GetCollection<Equipo>("division").Save(d4);

            l5.Divisiones.Add(d4.Id);

            Database.GetCollection<Equipo>("liga").Save(l5);
            //fin liga mmunera


            Liga l6 = new Liga();
            l6.Nombre = l6.Entrenador = "jvanegas";
            l6.Plan = "Freemium";
            l6.Usuarios.Add("jvanegas", "jvanegas@importacionesintegrales.com");

            i = 0;
            Equipo e6 = new Equipo();
            e6.Name = i.ToString();
            e6.Miembros.Add("jvanegas");

            Database.GetCollection<Equipo>("equipo").Save(e6);

            Division d5 = new Division();
            d5.Name = "Individual";
            d5.Equipos.Add(e6.Id);

            Database.GetCollection<Equipo>("division").Save(d5);


            l6.Divisiones.Add(d5.Id);
            Database.GetCollection<Equipo>("liga").Save(l6);
            //fin liga jvanegas

            Liga l7 = new Liga();
            l7.Nombre = l7.Entrenador = "laurablandon";
            l7.Plan = "Freemium";
            l7.Usuarios.Add("laurablandon", "laurablandon@importacionesintegrales.com");

            i = 0;
            Equipo e7 = new Equipo();
            e7.Name = i.ToString();
            e7.Miembros.Add("laurablandon");
            Database.GetCollection<Equipo>("equipo").Save(e7);

            Division d6 = new Division();
            d6.Name = "Individual";
            d6.Equipos.Add(e7.Id);
            Database.GetCollection<Equipo>("division").Save(d6);


            l7.Divisiones.Add(d6.Id);
            Database.GetCollection<Equipo>("liga").Save(l7);
            //fin liga laurablandon



            Liga l2 = new Liga();
            l2.Nombre = l2.Entrenador = "zeitgeist";
            l2.Plan = "Estandar";
            l2.Usuarios.Add("catalinadelacuesta", "delacuesta@hotmail.com");
            l2.Usuarios.Add("mdelacue", "mdelacue@gmail.com");
            l2.Usuarios.Add("jvanegas", "jvanegas@importacionesintegrales.com");
            l2.Usuarios.Add("mmunera", "mmunera@importacionesintegrales.com");
            l2.Usuarios.Add("sthejoker", "sthejoker@gmail.com");
            l2.Usuarios.Add("laurablandon", "laurablandon@importacionesintegrales.com");
            l2.Usuarios.Add("restrepo.cafe", "restrepo.cafe@gmail.com");
            l2.Usuarios.Add("ddo88", "ddo88@hotmail.com");

            Equipo h= new Equipo();
            h.Name = "Hombres";
            h.Miembros.Add("ddo88");
            h.Miembros.Add("restrepo.cafe");
            h.Miembros.Add("jvanegas");
            h.Miembros.Add("sthejoker");
            Database.GetCollection<Equipo>("equipo").Save(h);
            
            Equipo m = new Equipo();
            m.Name = "Mujeres";
            m.Miembros.Add("mmunera");
            m.Miembros.Add("laurablandon");
            m.Miembros.Add("catalinadelacuesta");
            m.Miembros.Add("mdelacue");
            Database.GetCollection<Equipo>("equipo").Save(m);

            Division dz= new Division();
            dz.Name = "Guerra de Géneros";
            dz.Descripcion = "Hombres vs Mujeres";
            dz.Equipos.Add(m.Id);
            dz.Equipos.Add(h.Id);

            Database.GetCollection<Equipo>("division").Save(dz);
            l2.Divisiones.Add(dz.Id);
            Database.GetCollection<Equipo>("liga").Save(l2);
            Equipo a= new Equipo();
            a.Name = "R&D";

            Database.GetCollection<Equipo>("equipo").Save(a);

            Equipo b= new Equipo();
            b.Name = "Contabilidad";
            Database.GetCollection<Equipo>("equipo").Save(b);
            Equipo c= new Equipo();
            c.Name = "Comercial";
            Database.GetCollection<Equipo>("equipo").Save(c);

            Division dy = new Division();
            dy.Name = "Areas";
            dy.Descripcion = "Areas de la compañía";
            dy.Equipos.Add(a.Id);
            dy.Equipos.Add(b.Id);
            dy.Equipos.Add(c.Id);

            Database.GetCollection<Equipo>("division").Save(dy);

            l2.Divisiones.Add(dy.Id);


            Division dx = new Division();
            dx.Name = "Edades";
            dx.Descripcion = "0-20, 20-30, 30-40, 40-50, 50-60";

            Database.GetCollection<Equipo>("division").Save(dx);
            l2.Divisiones.Add(dx.Id);

            Division dv = new Division();
            dv.Name = "Pisos";
            dv.Descripcion = "Ubicación en el Edificio";
            Database.GetCollection<Equipo>("division").Save(dv);
            l2.Divisiones.Add(dv.Id);

            Division du = new Division();
            du.Name = "Individual";
            du.Descripcion = "";
            Database.GetCollection<Equipo>("division").Save(du);
            l2.Divisiones.Add(du.Id);

            Database.GetCollection<Equipo>("liga").Save(l2);

            Console.WriteLine("Todo Good");
            // ------fin liga Zeitgeist ----

        }
    }


    
    public class ResponseRegistroProgreso : ResponseService, IReturn<RegistroProgreso>
    {

    }

    public class ResponseService
    {
        public string Message { get; set; }
        public bool State { get; set; }
    }

    [Route("/RegistroProgreso")]
    public class RegistroProgreso
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }
        public Int64 Pasos { get; set; }
        public double Calorias { get; set; }
        public DateTime Fecha { get; set; }
        public string IdReto { get; set; }
    }
   
}
