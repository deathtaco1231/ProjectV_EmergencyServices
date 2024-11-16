using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using Supabase;


namespace EmergencyServices.Group8
{
    public static class EmergencyBackend
    {
        public static Client supabase;
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

            ProcessedDisaster processedDisaster = BackendHelper.ConvertToProcessedDisaster(notif);
            return processedDisaster;
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

        public static bool VerifyUserReport(string usrReportJson)
        {
            UserDisasterReport deSerializedReport = JsonConvert.DeserializeObject<UserDisasterReport>(usrReportJson);
            return BackendHelper.VerifyUserReport(deSerializedReport);

        }

        //public static async Task LoadPdfContentToSupabase(string pdfFilePath)
        //{
        //    var disasterEntries = ParsePdfForDisasters(pdfFilePath);

        //    if (EmergencyBackend.supabase == null)
        //    {
        //        EmergencyBackend.Init();
        //    }

        //    foreach (var entry in disasterEntries)
        //    {
        //        var response = await EmergencyBackend.supabase
        //            .From<ProcessingInfo>()
        //            .Insert(entry);

        //        if (response.Models == null)
        //        {
        //            Console.WriteLine("Error inserting entry");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Disaster entry added successfully.");
        //        }
        //    }

        //}

        //internal static List<ProcessingInfo> ParsePdfForDisasters(string pdfFilePath)
        //{
        //    List<ProcessingInfo> disasters = new List<ProcessingInfo>();
        //    string currentDisasterType = string.Empty;
        //    string precautionSteps = string.Empty;
        //    string duringDisasterSteps = string.Empty;
        //    string recoverySteps = string.Empty;

        //    using (PdfReader reader = new PdfReader(pdfFilePath))
        //    {
        //        for (int page = 1; page <= reader.NumberOfPages; page++)
        //        {
        //            string pageText = PdfTextExtractor.GetTextFromPage(reader, page);
        //            var lines = pageText.Split('\n');

        //            for (int i = 0; i < lines.Length; i++)
        //            {
        //                var line = lines[i];

        //                if (line.StartsWith("- "))
        //                {
        //                    if (currentDisasterType != null)
        //                    {
        //                        disasters.Add(new ProcessingInfo
        //                        {
        //                            DisasterType = currentDisasterType,
        //                            PrecautionSteps = precautionSteps,
        //                            DuringDisasterSteps = duringDisasterSteps,
        //                            RecoverySteps = recoverySteps,
        //                            Timestamp = DateTime.Now
        //                        });

        //                        precautionSteps = string.Empty;
        //                        duringDisasterSteps = string.Empty;
        //                        recoverySteps = string.Empty;
        //                    }

        //                    currentDisasterType = line.TrimStart('-').Trim();
        //                }
        //                else if (line.Contains("Precaution Steps"))
        //                {
        //                    precautionSteps = CollectSection(lines, ref i, "Precaution Steps");
        //                }
        //                else if (line.Contains("In the event of"))
        //                {
        //                    duringDisasterSteps = CollectSection(lines, ref i, "In the event of");
        //                }
        //                else if (line.Contains("Recovery Steps"))
        //                {
        //                    recoverySteps = CollectSection(lines, ref i, "Recovery Steps");
        //                }
        //            }
        //        }

        //        if (currentDisasterType != null)
        //        {
        //            disasters.Add(new ProcessingInfo
        //            {
        //                DisasterType = currentDisasterType,
        //                PrecautionSteps = precautionSteps,
        //                DuringDisasterSteps = duringDisasterSteps,
        //                RecoverySteps = recoverySteps,
        //                Timestamp = DateTime.Now
        //            });
        //        }
        //    }

        //    return disasters;
        //}

        //private static string CollectSection(string[] lines, ref int index, string sectionHeader)
        //{
        //    var sectionText = new List<string>();
        //    bool collecting = false;

        //    for (; index < lines.Length; index++)
        //    {
        //        var line = lines[index];

        //        if (collecting)
        //        {
        //            if (line.StartsWith("- ") || line.Contains("Steps"))
        //                break;

        //            sectionText.Add(line.Trim());
        //        }

        //        if (line.Contains(sectionHeader))
        //        {
        //            collecting = true;
        //        }
        //    }

        //    return string.Join(" ", sectionText).Trim();
        //}


    }
}
