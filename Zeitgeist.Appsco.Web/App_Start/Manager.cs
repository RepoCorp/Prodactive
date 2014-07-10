using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeitgeist.Appsco.Web.Properties;

namespace Zeitgeist.Appsco.Web.App_Start
{
    public class Manager
    {
        public MongoDatabase Database;


        public Manager()
        {

            var client = new MongoClient(Settings.Default.ConnectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(Settings.Default.Bd);
        }

        

        private MongoCollection<T> GetCollection<T>(string collection)
        {
            var a = Database.GetCollection<T>(collection);
            return a;
        }

        public bool Save<T>(T item)
        {
            var wc=GetCollection<T>(Settings.Default.ClientCollection).Save(item);
            if (wc.Ok)
                return true;
            else
                return false;
        }


    }
}