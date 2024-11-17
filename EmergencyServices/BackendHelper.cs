using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    internal static class BackendHelper
    {
        public static List<ProcessingInfo> DisasterProcessingInfo;
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
                Source = notif.Source,
            };

            // Populate steps based on matching ProcessingInfo, or set to default steps if not found
            if (matchingInfo != null)
            {
                processedDisaster.PrecautionSteps = matchingInfo.PrecautionSteps;
                processedDisaster.DuringDisasterSteps = matchingInfo.DuringDisasterSteps;
                processedDisaster.RecoverySteps = matchingInfo.RecoverySteps;
            }
            else
            {
                processedDisaster.PrecautionSteps = "Listen to your local news station, review and practice evacuation routes, and make sure that your home and belongings are secured";
                processedDisaster.DuringDisasterSteps = "Watch for signs of a disaster and be prepared to evacuate or find proper shelter";
                processedDisaster.RecoverySteps = "Lookout for instructions from officials and community leaders, inspect your area for damages, listen to your local radio or news channel for further instructions";
            }

            return processedDisaster;
        }
    }
}
