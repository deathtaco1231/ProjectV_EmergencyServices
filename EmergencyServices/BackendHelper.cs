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
    }
}
