using System;
using System.Diagnostics;

namespace EmergencyServices.Group8
{
    public static class EmergencyBackend
    {
        public static Supabase.Client supabase;
        public static async void Init()
        {
            DotNetEnv.Env.Load();

            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            supabase = new Supabase.Client(url, key, options);
            await supabase.InitializeAsync();

            if (await BackendHelper.PopulateProcessingInfoList() == false)
            {
                Debug.WriteLine("ERROR: Failed to retrieve disaster processing data OR table was empty!"); // Change to console call if not in visual studio debug mode
                Environment.Exit(-1);
            }  
        }

        public static ProcessedDisaster ProcessNotification(string notifJson)
        {
            if (notifJson == null)
                return null;
            Notification notif = BackendHelper.JsonToNotification(notifJson);
            return /*Call to function Aidan is making here which processes notification into ProcessedDisaster*/ null; // REMOVE NULL LATER
        }

        private static ProcessedDisaster ConvertToProcessedDisaster(Notification notif)
        {
            return new ProcessedDisaster
            {
                Id = notif.Id,
                DisasterType = notif.DisasterType,
                Priority = notif.Priority,
                Description = notif.Description,
                PrecautionSteps = "Default Precaution Steps", 
                DuringDisasterSteps = "Default Disaster Steps", 
                RecoverySteps = "Default Recovery Steps", 
                Timestamp = notif.Timestamp,
                SeverityLevel = notif.SeverityLevel ?? 0, 
                Source = notif.Source
            };
        }
    }
}
