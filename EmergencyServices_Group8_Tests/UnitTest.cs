using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmergencyServices.Group8;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
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

        [TestMethod]
        public void Test1_AsyncSuccessful()
        {
            //if (EmergencyBackend.supabase != null)
            //    EmergencyBackend.supabase = null;

            //EmergencyBackend.Init();

            Assert.IsNotNull(EmergencyBackend.supabase);
        }

        [TestMethod]
        public async Task Test2_ClientRecievesTableContents()
        {
            var res = await EmergencyBackend.supabase.From<Testing>().Get();
            var models = res.Models;

            Assert.IsTrue(models.Count > 0);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Test4_ClientDeleteRowFromTable()
        {
            // Test requires that Test 3 passed. This checks for the post from that test and aborts if not found.
            var res = await EmergencyBackend.supabase.From<Testing>().Get();
            var models = res.Models;
            bool rowPresent = false;
            foreach (Testing t in models)
            {
                rowPresent = (t.TestString == "Test3String") ? true : false;
            }
            Assert.IsTrue(rowPresent); // Test fails if previous test was not successful

            await EmergencyBackend.supabase.From<Testing>().Where(x => x.TestString == "Test3String").Delete();
            var delRes = await EmergencyBackend.supabase.From<Testing>().Get();
            var delModels = delRes.Models;

            Assert.IsTrue(models.Count > delModels.Count);
        }

        [TestMethod]
        public async Task Test5_GetAllTestProcessedDisastersAsync_RetrievesAllEntries()
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
        public async Task Test6_GetAllTestProcessedDisastersAsync_EmptyTable()
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

        public async Task Test7_GetDisastersByPriorityAsync_RetrievesWatchPriority()
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
        public async Task Test8_GetDisastersByPriorityAsync_RetrievesWarningPriority()
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
        public async Task Test9_GetDisastersByPriorityAsync_RetrievesUrgentPriority()
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
        public async Task Test10_GetDisastersByPriorityAsync_RetrievesCriticalPriority()
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

        [TestMethod]
        public async Task Test6_RowCountStability()
        {
            // test to make sure that number of rows are consistent with multiple calls
            var initialRes = await EmergencyBackend.supabase.From<Testing>().Get();
            int initialCount = initialRes.Models.Count;

            var finalRes = await EmergencyBackend.supabase.From<Testing>().Get();
            int finalCount = finalRes.Models.Count;

            Assert.AreEqual(initialCount, finalCount);
        }

        [TestMethod]
        public async Task Test_MarkDisasterAsCriticalAsync_OnlyWhenUrgent()
        {
            if (EmergencyBackend.supabase == null)
            {
                EmergencyBackend.Init();
            }

            //Step 1: Insert a disaster with a non-'Urgent' priority
            var testDisaster = new TestProcessedDisaster
            {
                DisasterType = "Flood",
                Priority = "Warning", // Not 'Urgent'
                Description = "Test flood warning.",
                PrecautionSteps = "Test precaution steps.",
                DuringDisasterSteps = "Test during disaster steps.",
                RecoverySteps = "Test recovery steps.",
                Timestamp = DateTime.Now,
                SeverityLevel = 3.0,
                Source = "NWS"
            };

            var insertResponse = await EmergencyBackend.supabase.From<TestProcessedDisaster>().Insert(testDisaster);
            Assert.IsTrue(insertResponse.Models.Count > 0, "Failed to insert test disaster.");

            int disasterId = insertResponse.Models[0].Id;

            try
            {
                //Step 2: Attempt to mark it as 'Critical' (should fail)
                bool success = await BackendHelper.MarkTestDisasterAsCriticalAsync(disasterId);
                Assert.IsFalse(success, "Disaster was incorrectly updated to 'Critical' despite not being 'Urgent'.");

                //Step 3: Update the disaster to 'Urgent' and retry
                await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Update(new { Priority = "Urgent" });

                success = await BackendHelper.MarkTestDisasterAsCriticalAsync(disasterId);
                Assert.IsTrue(success, "Failed to update disaster to 'Critical' when priority was 'Urgent'.");
            }
            finally
            {
                //Cleanup: Remove the inserted disaster
                await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Delete();
            }
        }

        [TestMethod]
        public async Task Test_MarkDisasterAsCriticalAsync_UpdatesToCritical()
        {
            if (EmergencyBackend.supabase == null)
            {
                EmergencyBackend.Init();
            }

            //Step 1: Insert a disaster with 'Urgent' priority
            var testDisaster = new TestProcessedDisaster
            {
                DisasterType = "Hurricane",
                Priority = "Urgent", // This should allow it to update to 'Critical'
                Description = "Test hurricane warning.",
                PrecautionSteps = "Test precaution steps for hurricane.",
                DuringDisasterSteps = "Test during disaster steps for hurricane.",
                RecoverySteps = "Test recovery steps for hurricane.",
                Timestamp = DateTime.Now,
                SeverityLevel = 5.0,
                Source = "NWS"
            };

            var insertResponse = await EmergencyBackend.supabase.From<TestProcessedDisaster>().Insert(testDisaster);
            Assert.IsTrue(insertResponse.Models.Count > 0, "Failed to insert test disaster.");

            int disasterId = insertResponse.Models[0].Id;

            try
            {
                //Step 2: Attempt to mark it as 'Critical' (should succeed)
                bool success = await BackendHelper.MarkTestDisasterAsCriticalAsync(disasterId);
                Assert.IsTrue(success, "Failed to update disaster to 'Critical' when priority was 'Urgent'.");

                //Step 3: Verify the update
                var updatedDisasterResponse = await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Get();

                var updatedDisaster = updatedDisasterResponse.Models.FirstOrDefault();
                Assert.IsNotNull(updatedDisaster, "Updated disaster not found.");
                Assert.AreEqual("Critical", updatedDisaster.Priority, "Disaster priority was not updated to 'Critical'.");
            }
            finally
            {
                //Cleanup: Remove the inserted disaster
                await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Delete();
            }
        }

    }
}
