﻿//#define TESTING
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EmergencyServices.Group8 
{
    [ExcludeFromCodeCoverage]
    internal class Program // Likely will just be used for testing since all functionality will be in static class and methods and main will be in group 7's module
    {
        [ExcludeFromCodeCoverage]
        static async Task Main(string[] args)
        {
            Console.WriteLine("Initalizing EmergencyBackend...");
            EmergencyBackend.Init();
            Console.WriteLine("Initalizing Complete\n");

            Console.WriteLine("Retrieving all rows from testing table...\n");
            var testDisasters = await BackendHelper.GetAllTestProcessedDisastersAsync();
            foreach (var c in testDisasters)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("\nRetrieving rows from testing table with WARNING priority...\n");
            var newTestDisasters = await BackendHelper.GetTestDisastersByPriorityAsync(DisasterTypeEnums.Warning);
            foreach (var c in newTestDisasters)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("\nCreating Standard Notification...\n");
            NewNotificationFormat notifObj = new NewNotificationFormat();
            notifObj.notiforigin ="NWS";
            notifObj.disasterlevel = 2;
            notifObj.notifdate = DateTime.Now.ToString();
            notifObj.city = "Toronto";
            notifObj.disastertype = "Flooding";
            notifObj.latitude = 1.445f;
            notifObj.longitude = 2.937f;
            notifObj.id = 5;

            Console.WriteLine(notifObj.ToString());

            Console.WriteLine("\nConverting to JSON...\n");
            string jsonNotif = JsonConvert.SerializeObject(notifObj);
            Console.WriteLine(jsonNotif);

            Console.WriteLine("\nProcessing into ProcessedDisaster...\n");
            ProcessedDisaster procObj = EmergencyBackend.ProcessNotification(jsonNotif);
            string testJson = JsonConvert.SerializeObject(procObj);
            Console.WriteLine("\nTest Json of raw processed disaster:\n" + testJson + "\n");
            Console.WriteLine(procObj.ToString());

            string jsonProc = await EmergencyBackend.GetNewNotifFormatProcessedDisasterJson(procObj.Id);
            Console.WriteLine("\nProcessed Version JSON: \n");
            Console.WriteLine(jsonProc);

            // TESTING NESTED JSON
            //UserDisasterReport userDisasterReport = new UserDisasterReport();
            //userDisasterReport.report_id = 1;
            //userDisasterReport.user_id = 12;
            //userDisasterReport.location.latitude = 15.5;
            //userDisasterReport.location.address = "lol";
            //userDisasterReport.@event.type = "flood";
            //userDisasterReport.@event.severity = "bad as hellll";
            //userDisasterReport.media.type = "video";
            //userDisasterReport.media.description = "description of media here";

            //string usrReportJson = JsonConvert.SerializeObject(userDisasterReport);

            //UserDisasterReport deSerializedReport = JsonConvert.DeserializeObject<UserDisasterReport>(usrReportJson);

            DateTime test = DateTime.Now;
            //Thread.Sleep(1700);
            DateTime compare = DateTime.Now;
            int res = compare.CompareTo(test); // returns 1 no matter what, indicating that the object passed in holds date/time before the caller
            int res2 = test.CompareTo(compare); // returns -1 no matter what, indicating that the object passed in holds date/time after the caller

            Console.WriteLine((ulong)test.ToBinary() + " Example of binary time");

            DateTime day1 = DateTime.MinValue;
            DateTime day2 = DateTime.MinValue;
            day1 = day1.AddDays(1);
            ulong oneDayBinary = (ulong)day1.ToBinary();
            
            //UserDisasterReport firstTestReport = new UserDisasterReport();
            //firstTestReport.@event.type = "flooding";
            //firstTestReport.user_id = 23;
            //firstTestReport.created_at = DateTime.Now;
            //UserDisasterReport secondTestReport = new UserDisasterReport();
            //secondTestReport.@event.type = "OtherType";
            //secondTestReport.user_id = 23;
            //secondTestReport.created_at = DateTime.Now.AddMinutes(1);
            //UserDisasterReport thirdTestReport = new UserDisasterReport();
            //thirdTestReport.@event.type = "Flooding";
            //thirdTestReport.user_id = 25;
            //thirdTestReport.created_at = DateTime.Now.AddMinutes(1);

            //string firstTestReportJson = JsonConvert.SerializeObject(firstTestReport);
            //string secondTestReportJson = JsonConvert.SerializeObject(secondTestReport);
            //string thirdTestReportJson = JsonConvert.SerializeObject(thirdTestReport);

            //Console.WriteLine("Verification of first disaster: " + EmergencyBackend.VerifyUserReport(firstTestReportJson).ToString());
            //Console.WriteLine("Verification of second disaster: " + EmergencyBackend.VerifyUserReport(secondTestReportJson).ToString());
            //Console.WriteLine("Verification of third disaster: " + EmergencyBackend.VerifyUserReport(thirdTestReportJson).ToString());

            Console.WriteLine("Making procObj critical:");
            Console.WriteLine("Previous priority: " + procObj.Priority.ToString());

            if (await EmergencyBackend.MarkDisasterAsCriticalAsync(procObj.Id)==true)
                procObj.Priority = "Critical";

            Console.WriteLine("New priority: " + procObj.Priority.ToString());

            Console.WriteLine("Making procObj critical with previous priority set to urgent");


            Console.Read();

        }
    }
}
