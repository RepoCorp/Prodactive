using System;
using System.Linq;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeitgeist.Appsco.Web.Manage.Tests
{
    [TestClass()]
    public class OrquestratorTests
    {
        //[TestMethod()]
        public void VerificarRetosTest()
        {
            Orquestrator or = Orquestrator.Instance;
            or.VerificarRetos(new object());
            Assert.Fail();
        }

        [TestMethod()]
        public void CalcularLogroDiarioTest()
        {
            Manager m = Manager.Instance;
            Reto r = m.GetRetosActivos().First();
            try
            {
                Orquestrator.Instance.LogrosDiarios(r);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
            
            
        }



    }
}
