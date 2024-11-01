using Microsoft.VisualStudio.TestTools.UnitTesting;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Reflection;
using EmergencyServices.Group8;
using System;

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
        public void TestMethod1()
        {
            var stuff = EmergencyBackend.supabase.From<Testing>().Get();
        }
    }
}
