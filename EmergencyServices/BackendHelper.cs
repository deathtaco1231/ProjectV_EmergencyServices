﻿using Newtonsoft.Json;
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
                // Convert each ProcessingInfo disaster type to uppercase before comparison
                if (DisasterProcessingInfo[i].DisasterType.ToUpper() == notificationDisasterType)
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

            // Populate steps based on matching ProcessingInfo, or set to null if not found
            if (matchingInfo != null)
            {
                processedDisaster.PreparationSteps = matchingInfo.PrecautionSteps;
                processedDisaster.ActiveSteps = matchingInfo.DuringDisasterSteps;
                processedDisaster.RecoverySteps = matchingInfo.RecoverySteps;
            }
            else
            {
                processedDisaster.PreparationSteps = null;
                processedDisaster.ActiveSteps = null;
                processedDisaster.RecoverySteps = null;
            }

            return processedDisaster;
        }

        public static async Task<List<TestProcessedDisaster>> GetAllTestProcessedDisastersAsync()
        {
            try
            {
                //Query the 'test_disaster_processed' table
                var response = await supabase
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
                var response = await supabase
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
                var currentDisasterResponse = await supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Get();

                var currentDisaster = currentDisasterResponse.Models.FirstOrDefault();

                if (currentDisaster == null)
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

                var response = await supabase
                    .From<TestProcessedDisaster>()
                    .Where(d => d.Id == disasterId)
                    .Update(updatedDisaster);

                return response.Models.Count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error marking test disaster as critical: {ex.Message}");
                return false;
            }
        }

    }
}
