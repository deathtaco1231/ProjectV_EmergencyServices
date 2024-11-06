using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
        public static async Task<List<ProcessedDisaster>> GetAllProcessedDisastersAsync()
        {
            try
            {
                // Query the 'disaster_processed' table
                var response = await supabase
                    .From<ProcessedDisaster>()   // Target the production table model
                    .Select("*")
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving processed disasters from disaster_processed table: {ex.Message}");
                return new List<ProcessedDisaster>();
            }
        }

        public static async Task<List<TestProcessedDisaster>> GetAllTestProcessedDisastersAsync()
        {
            try
            {
                // Query the 'test_disaster_processed' table
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


        public static async Task<List<ProcessedDisaster>> GetDisastersByPriorityAsync(DisasterTypeEnums priorityLevel)
        {
            try
            {
                string priorityAsString = priorityLevel.ToString();
                Debug.WriteLine($"Filtering production table for priority: {priorityAsString}");

                // Query the 'disaster_processed' table to filter by priority level
                var response = await supabase
                    .From<ProcessedDisaster>()    // Target the production model
                    .Select("*")
                    .Filter(x => x.Priority, Postgrest.Constants.Operator.Equals, priorityAsString)
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving disasters with priority '{priorityLevel}' from production table: {ex.Message}");
                return new List<ProcessedDisaster>();
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

    }
}
