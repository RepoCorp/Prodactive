using System;
using Android.Util;
using ProdactiveMovil.ModelServicio;
using ProdactiveMovil.ModelServicio.SQLite;
using ServiceStack;
using SQLite.Net.Interop;

namespace ProdactiveMovil.Database
{
    public class Manager
    {
        public static Manager _instance;
        private Database Db { get; set; }
        private static readonly object _mutex = new object();

        public IServiceClient ServiceClient { get; set; }

        public string Path { get; set; }

        public static Manager GetInstance(ISQLitePlatform plataform, IServiceClient client, string path)
        {
            if (_instance == null)
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new Manager(plataform, client, path);
                    }
                }
            }
            return _instance;
        }


        private Manager(ISQLitePlatform platform, IServiceClient client, string databasePath)
        {
            //Db = new DatabaseAsync(platform, databasePath);
            Db = new Database(platform, databasePath);
            ServiceClient = client;
        }

        public int SavePersona(PersonaSql p)
        {
            try
            {
                return Db.InsertPersona(p);
            }
            catch (Exception ex)
            {
                Log.Error("SavePersona", ex.Message);
                return -1;
            }
        }
        

        //public int SaveRecetaService(Recetas rec)
        //{
        //    try
        //    {
        //        ResponseRecetas r = ServiceClient.Send<ResponseRecetas>(rec);
        //        return (r.State == true ? 1 : -1);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //}

        //public int SaveReceta(Receta rec)
        //{
        //    if (rec.Detalles.Count > 0)
        //        return Db.InsertReceta(rec);

        //    return -2;
        //}

        //public List<Receta> GetList()
        //{
        //    return Db.GetListRecetas();
        //}

        //public Receta GetUltimaReceta()
        //{
        //    return Db.LastReceta();
        //}

        internal int SaveReto(RetoSql retoSql)
        {
            try
            {
                return Db.InsertReto(retoSql);
            }
            catch (Exception ex)
            {
                Log.Error("SaveReto", ex.Message);
                return -1;
            }
        }

        public int SaveReporte(ReporteSql rep)
        {
            try
            {
                return Db.SaveReporte(rep);
            }
            catch (Exception ex)
            {
                Log.Error("SaveReto", ex.Message);
                return -1;
            }
            
        }

        public bool SendReporte(ReporteSql rep)
        {

            try
            {
                Log.Info("Send Reporte","");
                var r = ServiceClient.Send<ResponseRegistroProgreso>(rep.GetRegistroProgreso());
                if (SaveReporte(rep) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("SendReporte", ex.Message);
            }
            
            return false;
        }
    }


    public static class ExtensionMethod
    {

        public static RegistroProgreso GetRegistroProgreso(this ReporteSql reporte)
        {
            return new RegistroProgreso()
            {
                UserName = reporte.UserName,
                Fecha = reporte.Fecha,
                IdReto = reporte.IdReto,
                Pasos = reporte.Pasos,
                Calorias = reporte.Calorias
            };
        }
    }
}