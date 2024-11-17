using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    internal static class BackendHelper
    {
        public static List<ProcessingInfo> DisasterProcessingInfo;

        public static List<UserDisasterReport> UserDisasterReports = new List<UserDisasterReport>();

        private const ulong acceptedTimeDiffClauseOne = 36000000000; // currently single hour binary 

        private const ulong acceptedTimeDiffClauseTwo = 3000000000; // currently 5 minute binary
        public static Notification JsonToNotification(string json)
        {
            dynamic jsonContents = JObject.Parse(json);
            Notification newNotif = new Notification();
            newNotif.Id = jsonContents.Id;
            newNotif.Timestamp = jsonContents.Timestamp;
            newNotif.DisasterType = jsonContents.DisasterType;
            newNotif.Priority = jsonContents.Priority;
            newNotif.Description = jsonContents.Description;
            newNotif.SeverityLevel = jsonContents.SeverityLevel;
            newNotif.Source = jsonContents.Source;
            return newNotif;
        }

        public static string ProcessedDisasterToJson(ProcessedDisaster p)
        {
            return JsonConvert.SerializeObject(p);
        }

        public static async Task<bool> PopulateProcessingInfoList()
        {
            DisasterProcessingInfo = new List<ProcessingInfo>();
            var res = await EmergencyBackend.supabase.From<ProcessingInfo>().Get();
            var models = res.Models;
            if (models.Count == 0) 
                return false;
            foreach (ProcessingInfo p in models) {
                DisasterProcessingInfo.Add(p);
            }
            return true;
        }

        internal static ProcessedDisaster ConvertToProcessedDisaster(Notification notif)
        {
            // Convert the notification's disaster type to uppercase for case-insensitive matching
            string notificationDisasterType = notif.DisasterType.ToUpper();

            ProcessingInfo matchingInfo = null;

            // Loop through each entry in DisasterProcessingInfo to find a match
            for (int i = 0; i < DisasterProcessingInfo.Count; i++)
            {
                // Retrieve the disaster type from the database, convert it to uppercase, and compare
                string databaseDisasterType = DisasterProcessingInfo[i].DisasterType.ToUpper();
                if (databaseDisasterType == notificationDisasterType)
                {
                    matchingInfo = DisasterProcessingInfo[i];
                    break;
                }
            }

            // Create a new ProcessedDisaster object and copy info from Notification
            var processedDisaster = new ProcessedDisaster
            {
                DisasterType = notif.DisasterType,
                Priority = notif.Priority,
                Description = notif.Description,
                SeverityLevel = notif.SeverityLevel ?? 0,
                Source = notif.Source
            };

            // Populate steps based on matching ProcessingInfo, or set to default steps if not found
            if (matchingInfo != null)
            {
                processedDisaster.PreparationSteps = matchingInfo.PrecautionSteps;
                processedDisaster.ActiveSteps = matchingInfo.DuringDisasterSteps;
                processedDisaster.RecoverySteps = matchingInfo.RecoverySteps;
            }
            else
            {
                processedDisaster.PreparationSteps = "Listen to your local news station, review and practice evacuation routes, and make sure that your home and belongings are secured";
                processedDisaster.ActiveSteps = "Watch for signs of a disaster and be prepared to evacuate or find proper shelter";
                processedDisaster.RecoverySteps = "Lookout for instructions from officials and community leaders, inspect your area for damages, listen to your local radio or news channel for further instructions";

            }

            return processedDisaster;
        }
      
        internal static bool VerifyUserReport(UserDisasterReport usrReport)
        {
            if (usrReport == null)
                return false;

            foreach (UserDisasterReport r in UserDisasterReports)
            {
                if ((r.user_id == usrReport.user_id && DateSeperation(usrReport.created_at, r.created_at, acceptedTimeDiffClauseOne) == false) || (String.Compare(r.@event.type.ToUpper(), usrReport.@event.type.ToUpper()) == 0 && DateSeperation(usrReport.created_at, r.created_at, acceptedTimeDiffClauseTwo) == false))
                    return false;        
            }

            UserDisasterReports.Add(usrReport); // only add to list if approved in order to adhere to spam timing rules accurately 
            return true;
        }
        internal static bool DateSeperation(DateTime incoming, DateTime existing, ulong diff) // true if greater than time limit set
        {
            if (((ulong)incoming.ToBinary() - (ulong)existing.ToBinary()) > diff)
                return true;
            return false;
        }

        public static async Task<List<TestProcessedDisaster>> GetAllTestProcessedDisastersAsync()
        {
            try
            {
                //Query the 'test_disaster_processed' table
                var response = await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()   // Target the test table model
                    .Select("*")
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving processed disasters from test_disaster_processed table: {ex.Message}");
                return new List<TestProcessedDisaster>();
            }
        }

        public static async Task<List<TestProcessedDisaster>> GetTestDisastersByPriorityAsync(DisasterTypeEnums priorityLevel)
        {
            try
            {
                string priorityAsString = priorityLevel.ToString();
                Debug.WriteLine($"Filtering test table for priority: {priorityAsString}");

                // Query the 'test_disaster_processed' table to filter by priority level
                var response = await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()    // Target the test model
                    .Select("*")
                    .Filter(x => x.Priority, Postgrest.Constants.Operator.Equals, priorityAsString)
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving disasters with priority '{priorityLevel}' from test table: {ex.Message}");
                return new List<TestProcessedDisaster>();
            }
        }

        public static async Task<bool> MarkTestDisasterAsCriticalAsync(int disasterId)
        {
            try
            {
                // Retrieve the current disaster by ID
                var currentDisasterResponse = await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Get();

                var currentDisaster = currentDisasterResponse.Models.FirstOrDefault();

                if (currentDisaster == null || currentDisasterResponse.Models.Count == 0)
                {
                    Debug.WriteLine("Test disaster not found.");
                    return false;
                }

                if (currentDisaster.Priority != "Urgent")
                {
                    Debug.WriteLine("Test disaster priority is not 'Urgent', cannot update to 'Critical'.");
                    return false;
                }

                // Update the priority to 'Critical'
                var updatedDisaster = new { Priority = "Critical" };

                var response = await EmergencyBackend.supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Set(d => d.Priority, "Critical")
                    .Update();

                return /*response.Models.Count > 0*/ true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error marking test disaster as critical: {ex.Message}");
                return false;
            }
        }
    }
}
