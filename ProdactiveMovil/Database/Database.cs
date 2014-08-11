using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Android.Util;
using Org.W3c.Dom;
using ProdactiveMovil.ModelServicio;
using ProdactiveMovil.ModelServicio.SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace ProdactiveMovil.Database
{
    public class Database : SQLiteConnection
    {
        public Database(ISQLitePlatform plataform, string path) : base(plataform, path)
        {
            CreateTable<PersonaSql>();
            CreateTable<RetoSql>();
            CreateTable<ReporteSql>();
            //CreateTable<RegistroProgreso>();
        }

        //public int InsertReceta(Receta r)
        //{
        //    try
        //    {
        //        this.Insert(r);
        //        this.Commit();
        //        int id = GetLastInsertedReceta();
        //        foreach (var a in r.Detalles)
        //        {
        //            a.RecetaId = id;
        //            this.Insert(a);
        //            this.Commit();
        //        }
        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        return -1;
        //    }
        //}

        //public int GetLastInsertedReceta()
        //{
        //    var a = Table<Receta>().OrderByDescending(x => x.ID).ToList();
        //    Receta b = null;
        //    if (a.Count > 0)
        //        b = a.First();
        //    if (b != null)
        //        return b.ID;
        //    return -1;
        //}

        //public Receta LastReceta()
        //{
        //    Receta r = Table<Receta>().OrderByDescending(x => x.ID).First();
        //    r.Detalles = Table<AlarmItem>().Where(x => x.RecetaId == r.ID).ToList();
        //    return r;
        //}

        public PersonaSql GetPersona()
        {
            return Table<PersonaSql>().First();
        }

        public int InsertPersona(PersonaSql persona)
        {
            try
            {

                this.Insert(persona);
                this.Commit();
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        internal int InsertReto(RetoSql retoSql)
        {
            try
            {
               return VerifyInsert<RetoSql>(
                    x => x.Id == retoSql.Id, 
                    retoSql, 
                    (x, y) => x.IsActivo != y.IsActivo, 
                    (x, y) => { x.IsActivo = y.IsActivo; return x; });
                //var query = Table<RetoSql>().Where(x => x.Id == retoSql.Id).ToList();
                //if ( query.Count() == 0)
                //{
                //    this.Insert(retoSql);
                //    this.Commit();
                //    return 1;
                //}
                //RetoSql ant = query.First();
                //if (ant.IsActivo != retoSql.IsActivo)
                //{
                //    ant.IsActivo = retoSql.IsActivo;
                //    Update(ant);
                //    return 1;
                //}
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        internal int VerifyInsert<T>(Expression<Func<T,bool>> func, T newElement,Func<T,T,bool> func1,Func<T,T,T> elementUpdate ) where T: new ()
        {
            try
            {
                List<T> query = Table<T>().Where(func).ToList();
                if ( query.Count() == 0)
                {
                    this.Insert(newElement);
                    this.Commit();
                    return 1;
                }
                T ant = query.First();
                
                if (func1(ant, newElement))
                {
                    Update(elementUpdate(ant,newElement));
                    return 1;
                }
            }
            catch (Exception e)
            {
                Log.Error("VerifyInsert", e.Message);
            }
            return -1;
        }

        public int SaveReporte(ReporteSql rep)
        {
            try
            {
                this.Insert(rep);
                this.Commit();
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }

    public class DatabaseAsync : SQLiteAsyncConnection
    {
        public DatabaseAsync(ISQLitePlatform plataform, string path):base (new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(plataform, new SQLiteConnectionString(path, storeDateTimeAsTicks: false))))
        {
            CreateTableAsync<Persona>();
        }

        //public int InsertReceta(Receta r)
        //{
        //    try
        //    {
        //        this.Insert(r);
        //        this.Commit();
        //        int id = GetLastInsertedReceta();
        //        foreach (var a in r.Detalles)
        //        {
        //            a.RecetaId = id;
        //            this.Insert(a);
        //            this.Commit();
        //        }
        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        return -1;
        //    }
        //}

        //public int GetLastInsertedReceta()
        //{
        //    var a = Table<Receta>().OrderByDescending(x => x.ID).ToList();
        //    Receta b = null;
        //    if (a.Count > 0)
        //        b = a.First();
        //    if (b != null)
        //        return b.ID;
        //    return -1;
        //}

        //public Receta LastReceta()
        //{
        //    Receta r = Table<Receta>().OrderByDescending(x => x.ID).First();
        //    r.Detalles = Table<AlarmItem>().Where(x => x.RecetaId == r.ID).ToList();
        //    return r;
        //}

        public Task<Persona> GetPersona()
        {
            return Table<Persona>().FirstAsync();
        }

        public Task<int> InsertPersona(Persona persona)
        {
            try
            {
                return InsertAsync(persona);
            }
            catch (Exception e)
            {
                return Task.Factory.StartNew(() => -1);
            }
        }
    }
}