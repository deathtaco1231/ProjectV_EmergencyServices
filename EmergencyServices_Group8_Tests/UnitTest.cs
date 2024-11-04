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
    public class UnitTests
    {
        //[TestInitialize]
        //public void SetUp()
        //{
        //    EmergencyBackend.Init();
        //}

        [TestMethod]
        public void Test1_AsyncSuccessful()
        {
            if (EmergencyBackend.supabase != null)
                EmergencyBackend.supabase = null;

            EmergencyBackend.Init();

            Assert.IsNotNull(EmergencyBackend.supabase);
        }
        [TestMethod]
        public async Task Test2_ClientRecievesTableContents()
        {
            var res = await EmergencyBackend.supabase.From<Testing>().Get();
            var models = res.Models;

            Assert.IsTrue(models.Count > 0);
        }
        public async Task Test3_ClientPosteRowToTable()
        {
            var res = await EmergencyBackend.supabase.From<Testing>().Get();
            var models = res.Models;
            int sizeBefore = models.Count;
            var model = new Testing
            {
                TestString = "Test3String"
            };
            await EmergencyBackend.supabase.From<Testing>().Insert(model);
            var newRes = await EmergencyBackend.supabase.From<Testing>().Get();
            var newModels = newRes.Models;

        }
        public async Task Test4_ClientDeleteRowFromTable()
        {
            // Test requires that Test 3 passed. This checks for the post from that test and aborts if not found.
            var res = await EmergencyBackend.supabase.From<Testing>().Get(); 
            var models = res.Models;
            bool rowPresent = false;
            foreach(Testing t in models)
            {
                rowPresent = (t.TestString == "Test3String") ? true : false;   
            }
            Assert.IsTrue(rowPresent); // Test fails if previous test was not successful

            await EmergencyBackend.supabase.From<Testing>().Where(x=>x.TestString == "Test3String").Delete();
            var delRes = await EmergencyBackend.supabase.From<Testing>().Get();
            var delModels = delRes.Models;
            
            Assert.IsTrue(models.Count > delModels.Count);
        }
    }
}
