using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmergencyServices.Group8
{
    internal class NewNotificationFormat // NEW NOTIFICATION STRUCTURE WE JUST FOUND OUT ABOUT LOLZ
    {
        public int id { get; set; } // Keep this for the database, but do not send it to EMS.

        public string notiforigin { get; set; } = string.Empty; // Initialized

        public float longitude { get; set; }

        public float latitude { get; set; }

        public string city { get; set; } = string.Empty; // Initialized

        public string disastertype { get; set; } = string.Empty; // Initialized

        public int disasterlevel { get; set; }

        public string notifdate { get; set; } = string.Empty; // Initialized

        public string preparationsteps { get; set; } // TO BE POPULATED BY US
        public string activesteps { get; set; } // TO BE POPULATED BY US
        public string recoverysteps { get; set; } // TO BE POPULATED BY US
    }
}
