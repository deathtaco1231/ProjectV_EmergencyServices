using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmergencyServices.Group8;
using System.Threading.Tasks;
using System.Diagnostics;
//public async Task Test1_AsyncSuccessful() When making a test that requires async
//public void Test1_AsyncSuccessful() for any other test (except when specific type required)
namespace EmergencyServices_Group8_Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestInitialize]
        public void SetUp()
        {
            EmergencyBackend.Init();
        }

        //[TestMethod]
        //public void Test1_AsyncSuccessful()
        //{
        //    //if (EmergencyBackend.supabase != null)
        //    //    EmergencyBackend.supabase = null;

        //    //EmergencyBackend.Init();

        //    Assert.IsNotNull(EmergencyBackend.supabase);
        //}
        //[TestMethod]
        //public async Task Test2_ClientRecievesTableContents()
        //{
        //    var res = await EmergencyBackend.supabase.From<Testing>().Get();
        //    var models = res.Models;

        //    Assert.IsTrue(models.Count > 0);
        //}
        //[TestMethod]
        //public async Task Test3_ClientPosteRowToTable()
        //{
        //    var res = await EmergencyBackend.supabase.From<Testing>().Get();
        //    var models = res.Models;
        //    int sizeBefore = models.Count;
        //    var model = new Testing
        //    {
        //        TestString = "Test3String"
        //    };
        //    await EmergencyBackend.supabase.From<Testing>().Insert(model);
        //    var newRes = await EmergencyBackend.supabase.From<Testing>().Get();
        //    var newModels = newRes.Models;

        //}
        //[TestMethod]
        //public async Task Test4_ClientDeleteRowFromTable()
        //{
        //    // Test requires that Test 3 passed. This checks for the post from that test and aborts if not found.
        //    var res = await EmergencyBackend.supabase.From<Testing>().Get(); 
        //    var models = res.Models;
        //    bool rowPresent = false;
        //    foreach(Testing t in models)
        //    {
        //        rowPresent = (t.TestString == "Test3String") ? true : false;   
        //    }
        //    Assert.IsTrue(rowPresent); // Test fails if previous test was not successful

        //    await EmergencyBackend.supabase.From<Testing>().Where(x=>x.TestString == "Test3String").Delete();
        //    var delRes = await EmergencyBackend.supabase.From<Testing>().Get();
        //    var delModels = delRes.Models;

        //    Assert.IsTrue(models.Count > delModels.Count);
        //}

        [TestMethod]
        public async Task Test_GetAllTestProcessedDisastersAsync_RetrievesAllEntries()
        {
            //Retrieve all disasters from the test table
            var allDisasters = await EmergencyBackend.GetAllTestProcessedDisastersAsync();

            //Assert that data was retrieved and that the table is not empty
            Assert.IsNotNull(allDisasters, "The retrieved data is null.");
            Assert.IsTrue(allDisasters.Count > 0, "No data retrieved from test_disaster_processed table.");

            //Output each disaster for verification
            foreach (var disaster in allDisasters)
            {
                Debug.WriteLine($"ID: {disaster.Id}, Type: {disaster.DisasterType}, Priority: {disaster.Priority}");
            }
        }

        [TestMethod]
        public async Task Test_GetAllTestProcessedDisastersAsync_EmptyTable()
        {
            //Backup existing data
            var originalData = await EmergencyBackend.GetAllTestProcessedDisastersAsync();

            //Clear the table by deleting each record in the test data
            foreach (var record in originalData)
            {
                await EmergencyBackend.supabase.From<TestProcessedDisaster>().Delete(record);
            }

            //Test that the table is now empty
            var emptyDisasters = await EmergencyBackend.GetAllTestProcessedDisastersAsync();
            Assert.IsNotNull(emptyDisasters);
            Assert.AreEqual(0, emptyDisasters.Count, "Expected no data in test_disaster_processed table.");

            //Restore the original data
            foreach (var record in originalData)
            {
                await EmergencyBackend.supabase.From<TestProcessedDisaster>().Insert(record);
            }
        }

        [TestMethod]
        public async Task Test_GetDisastersByPriorityAsync_RetrievesWatchPriority()
        {
            //Retrieve disasters with "Watch" priority
            var watchDisasters = await EmergencyBackend.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Watch);

            //Assert that all retrieved disasters have the "Watch" priority
            Assert.IsNotNull(watchDisasters, "The retrieved data is null.");
            Assert.IsTrue(watchDisasters.Count > 0, "No disasters found with the 'Watch' priority.");

            foreach (var disaster in watchDisasters)
            {
                Assert.AreEqual("Watch", disaster.Priority, "Disaster priority does not match 'Watch'.");
                Debug.WriteLine($"ID: {disaster.Id}, Type: {disaster.DisasterType}, Priority: {disaster.Priority}");
            }
        }

        [TestMethod]
        public async Task Test_GetDisastersByPriorityAsync_RetrievesWarningPriority()
        {
            //Retrieve disasters with "Warning" priority
            var warningDisasters = await EmergencyBackend.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Warning);

            Assert.IsNotNull(warningDisasters, "The retrieved data is null.");
            Assert.IsTrue(warningDisasters.Count > 0, "No disasters found with the 'Warning' priority.");

            foreach (var disaster in warningDisasters)
            {
                Assert.AreEqual("Warning", disaster.Priority, "Disaster priority does not match 'Warning'.");
                Debug.WriteLine($"ID: {disaster.Id}, Type: {disaster.DisasterType}, Priority: {disaster.Priority}");
            }
        }

        [TestMethod]
        public async Task Test_GetDisastersByPriorityAsync_RetrievesUrgentPriority()
        {
            //Retrieve disasters with "Urgent" priority
            var urgentDisasters = await EmergencyBackend.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Urgent);

            Assert.IsNotNull(urgentDisasters, "The retrieved data is null.");
            Assert.IsTrue(urgentDisasters.Count > 0, "No disasters found with the 'Urgent' priority.");

            foreach (var disaster in urgentDisasters)
            {
                Assert.AreEqual("Urgent", disaster.Priority, "Disaster priority does not match 'Urgent'.");
                Debug.WriteLine($"ID: {disaster.Id}, Type: {disaster.DisasterType}, Priority: {disaster.Priority}");
            }
        }

        [TestMethod]
        public async Task Test_GetDisastersByPriorityAsync_RetrievesCriticalPriority()
        {
            //Retrieve disasters with "Critical" priority
            var criticalDisasters = await EmergencyBackend.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Critical);

            Assert.IsNotNull(criticalDisasters, "The retrieved data is null.");
            Assert.IsTrue(criticalDisasters.Count > 0, "No disasters found with the 'Critical' priority.");

            foreach (var disaster in criticalDisasters)
            {
                Assert.AreEqual("Critical", disaster.Priority, "Disaster priority does not match 'Critical'.");
                Debug.WriteLine($"ID: {disaster.Id}, Type: {disaster.DisasterType}, Priority: {disaster.Priority}");
            }
        }
    }
}
