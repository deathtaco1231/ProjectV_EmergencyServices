using Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    internal class Notification // This is an exact match of the Notification issued to us by the NWS, which // OLD STRUCT
    {
        public int Id { get; set; }
        public string DisasterType { get; set; }
        public string Priority { get; set; }  // Watch, Warning, Urgent, Critical
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public double? SeverityLevel { get; set; }  // E.g., Rainfall in mm, Hurricane category
        public string Source { get; set; }  // NWS or Emergency Services
        public float Latitude { get; set; } // NEW
        public float Longitude { get; set; } // NEW

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Id + " " + DisasterType + " " + Description + " " + Timestamp.ToString() + " " + SeverityLevel + " " + Source + "\n";
        }
    }
    //internal class Notification
    //{
    //    public int Id { get; set; }
    //    public string NotifOrigin { get; set; }
    //    public float Longitude { get; set; }
    //    public float Latitude { get; set; }
    //    public string City { get; set; }
    //    public string DisasterType { get; set; }
    //    public int DisasterLevel { get; set; }
    //    public string NotifDate { get; set; }
    //}
}
