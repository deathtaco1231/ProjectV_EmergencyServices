using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    internal class Notification // This is an exact match of the Notification issued to us by the NWS, which 
    {
        public int Id { get; set; }
        public string DisasterType { get; set; }
        public string Priority { get; set; }  // Watch, Warning, Urgent, Critical
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public double? SeverityLevel { get; set; }  // E.g., Rainfall in mm, Hurricane category
        public string Source { get; set; }  // NWS or Emergency Services
    }
}
