using Microsoft.VisualStudio.TestTools.UnitTesting;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Reflection;
using EmergencyServices.Group8;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EmergencyServices_Group8_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void SetUp()
        {
            EmergencyBackend.Init();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var res = await EmergencyBackend.supabase.From<Testing>().Get();
            var stuff = res.Models;
            Assert.IsTrue(stuff.Count > 0);
        }
    }
}
