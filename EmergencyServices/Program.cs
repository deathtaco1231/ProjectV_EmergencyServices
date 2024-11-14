//#define TESTING
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EmergencyServices.Group8 
{
    internal class Program // Likely will just be used for testing since all functionality will be in static class and methods and main will be in group 7's module
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Initalizing EmergencyBackend...");
            EmergencyBackend.Init();
            Console.WriteLine("Initalizing Complete\n");

            Console.WriteLine("Retrieving all rows from testing table...\n");
            var testDisasters = await EmergencyBackend.GetAllTestProcessedDisastersAsync();
            foreach (var c in testDisasters)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("\nRetrieving rows from testing table with WARNING priority...\n");
            var newTestDisasters = await EmergencyBackend.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Warning);
            foreach (var c in newTestDisasters)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("\nCreating Standard Notification...\n");
            Notification notifObj = new Notification();
            notifObj.Id = 1;
            notifObj.Source ="NWS";
            notifObj.Priority = "Warning";
            notifObj.Timestamp = DateTime.Now;
            notifObj.SeverityLevel = 11.5;
            notifObj.DisasterType = "Flooding";
            notifObj.Description = "Lots of water because its a flood";

            Console.WriteLine(notifObj.ToString());

            Console.WriteLine("\nConverting to JSON...\n");
            string jsonNotif = JsonConvert.SerializeObject(notifObj);
            Console.WriteLine(jsonNotif);

            Console.WriteLine("\nProcessing into ProcessedDisaster...\n");
            ProcessedDisaster procObj = EmergencyBackend.ProcessNotification(jsonNotif);
            Console.WriteLine(procObj.ToString());

            string jsonProc = JsonConvert.SerializeObject(procObj);

            Console.Read();
        }
    }
}
