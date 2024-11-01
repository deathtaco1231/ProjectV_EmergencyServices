//#define TESTING
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8 // Likely will just be used for testing since all functionality will be in static class and methods and main will be in group 7's module
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            EmergencyBackend.Init();

            //var model = new Testing
            //{
            //    TestString = "hem low"
            //};
            //await EmergencyBackend.supabase.From<Testing>().Insert(model);

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

            var model = new ForumPost
            {
                userName = "Test Insert User Name",
                postHeader = "Test Insert Post Header",
                postBody = "Test Insert Post Body",
            };
            await EmergencyBackend.supabase.From<ForumPost>().Insert(model);
            Console.Read();
        }
    }
}
