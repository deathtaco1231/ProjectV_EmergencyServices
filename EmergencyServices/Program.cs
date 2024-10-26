//#define TESTING
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8 // Likely will just be used for testing since 
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var supabase = new Supabase.Client(url, key, options);
            await supabase.InitializeAsync();

            var result = await supabase.From<ForumPost>().Get();
            var cities = result.Models;

            if (cities.Count == 0) // this for testing
                Console.WriteLine("No rows.");
            else // this also for testing
            {
                foreach (var c in cities)
                {
                    Console.WriteLine(c.ToString());
                }
            }
#if TESTING
            var model = new ForumPost
            {
                userName = "Test Insert User Name",
                postHeader = "Test Insert Post Header",
                postBody = "Test Insert Post Body",
                createdAt = DateTime.Now
            };
            await supabase.From<ForumPost>().Insert(model);
            #endif
            Console.Read();
        }
    }
}
