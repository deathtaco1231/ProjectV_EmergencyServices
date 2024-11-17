using Microsoft.VisualStudio.TestTools.UnitTesting;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Reflection;
using EmergencyServices.Group8;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
//public async Task Test1_AsyncSuccessful() When making a test that requires async
//public void Test1_AsyncSuccessful() for any other test (except when specific type required)
namespace EmergencyServices_Group8_Tests
{
    [TestClass]
    public class IntegrationTests
    {
        //[TestInitialize]
        //public void SetUp()
        //{
        //    EmergencyBackend.Init();
        //}

        [TestMethod]
        public void Test1_AsyncSuccessful()
        {
            //if (EmergencyBackend.supabase != null)
            //    EmergencyBackend.supabase = null;

            //EmergencyBackend.Init();

            //Assert.IsNotNull(EmergencyBackend.supabase);
            //var res = await EmergencyBackend.supabase.From<Testing>().Get();
            //var stuff = res.Models;
            //Assert.IsTrue(stuff.Count > 0);
        }
    }
}
