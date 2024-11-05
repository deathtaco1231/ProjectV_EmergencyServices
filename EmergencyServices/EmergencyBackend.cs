using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

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

        public static async Task LoadPdfContentToSupabase(string pdfFilePath)
        {
            var disasterEntries = ParsePdfForDisasters(pdfFilePath);

            if (EmergencyBackend.supabase == null)
            {
                EmergencyBackend.Init();
            }

            foreach (var entry in disasterEntries)
            {
                var response = await EmergencyBackend.supabase
                    .From<ProcessingInfo>()
                    .Insert(entry);

                if (response.Models == null)
                {
                    Console.WriteLine("Error inserting entry");
                }
                else
                {
                    Console.WriteLine("Disaster entry added successfully.");
                }
            }

        }

        internal static List<ProcessingInfo> ParsePdfForDisasters(string pdfFilePath)
        {
            List<ProcessingInfo> disasters = new List<ProcessingInfo>();
            string currentDisasterType = string.Empty;
            string precautionSteps = string.Empty;
            string duringDisasterSteps = string.Empty;
            string recoverySteps = string.Empty;

            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, page);
                    var lines = pageText.Split('\n');

                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];

                        if (line.StartsWith("- "))
                        {
                            if (currentDisasterType != null)
                            {
                                disasters.Add(new ProcessingInfo
                                {
                                    DisasterType = currentDisasterType,
                                    PrecautionSteps = precautionSteps,
                                    DuringDisasterSteps = duringDisasterSteps,
                                    RecoverySteps = recoverySteps,
                                    Timestamp = DateTime.Now
                                });

                                precautionSteps = string.Empty;
                                duringDisasterSteps = string.Empty;
                                recoverySteps = string.Empty;
                            }

                            currentDisasterType = line.TrimStart('-').Trim();
                        }
                        else if (line.Contains("Precaution Steps"))
                        {
                            precautionSteps = CollectSection(lines, ref i, "Precaution Steps");
                        }
                        else if (line.Contains("In the event of"))
                        {
                            duringDisasterSteps = CollectSection(lines, ref i, "In the event of");
                        }
                        else if (line.Contains("Recovery Steps"))
                        {
                            recoverySteps = CollectSection(lines, ref i, "Recovery Steps");
                        }
                    }
                }

                if (currentDisasterType != null)
                {
                    disasters.Add(new ProcessingInfo
                    {
                        DisasterType = currentDisasterType,
                        PrecautionSteps = precautionSteps,
                        DuringDisasterSteps = duringDisasterSteps,
                        RecoverySteps = recoverySteps,
                        Timestamp = DateTime.Now
                    });
                }
            }

            return disasters;
        }

        private static string CollectSection(string[] lines, ref int index, string sectionHeader)
        {
            var sectionText = new List<string>();
            bool collecting = false;

            for (; index < lines.Length; index++)
            {
                var line = lines[index];

                if (collecting)
                {
                    if (line.StartsWith("- ") || line.Contains("Steps"))
                        break;

                    sectionText.Add(line.Trim());
                }

                if (line.Contains(sectionHeader))
                {
                    collecting = true;
                }
            }

            return string.Join(" ", sectionText).Trim();
        }

    }
}
