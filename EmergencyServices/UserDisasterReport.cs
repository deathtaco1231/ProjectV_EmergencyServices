using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    internal class UserDisasterReport
    {
        [ExcludeFromCodeCoverage]
        public UserDisasterReport() 
        { 
            location = new Location();
            @event = new Event();
            media = new Media();
        }
        public int report_id { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public Location location { get; set; }
        public Event @event { get; set; }
        public Media media { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string address { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class Event
    {
        public string type { get; set; }
        public string severity { get; set; }
        public string description { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class Media
    {
        public string type { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }
}
