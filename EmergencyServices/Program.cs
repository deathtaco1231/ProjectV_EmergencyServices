//#define TESTING
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EmergencyServices.Group8 
{
    internal class Program // Likely will just be used for testing since all functionality will be in static class and methods and main will be in group 7's module
    {
        static async Task Main(string[] args)
        {
            EmergencyBackend.Init();

            var result = await EmergencyBackend.supabase.From<Testing>().Get();
            var strings = result.Models;
            if (strings.Count == 0) // this for testing
                Console.WriteLine("No rows.");
            else // this also for testing
            {
                foreach (var c in strings)
                {
                    Console.WriteLine(c.ToString());
                }
            }

            //ProcessedDisaster test = new ProcessedDisaster();
            //test.Id = 1;
            //test.DisasterType = "type here";
            //test.Priority = "priority";
            //test.DuringDisasterSteps = "during steps";
            //test.RecoverySteps = "recovery here";
            //test.PrecautionSteps = "precaution here";
            //test.Source = "source";
            //test.Description = "description";
            //test.SeverityLevel = 15.5;
            //string jsonTest = JsonConvert.SerializeObject(test);

            //dynamic jsonContents = JObject.Parse(jsonTest);
            //ProcessedDisaster newDisaster = new ProcessedDisaster();
            //newDisaster.Id = jsonContents.Id;
            //newDisaster.DisasterType = jsonContents.DisasterType;
            //newDisaster.Priority = jsonContents.Priority;
            //// so on and so forth

            Console.Read();
        }
    }
}
