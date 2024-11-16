using Postgrest.Models;
using Postgrest.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmergencyServices.Group8 // Created as part of ticket 105, for use in ticket 107
{
    [ExcludeFromCodeCoverage]
    [Table("disaster_emergency_data")]
    internal class ProcessingInfo : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Column("disaster_type")]
        public string DisasterType { get; set; }

        [Column("precaution_steps")] 
        public string PrecautionSteps { get; set; }

        [Column("during_disaster_steps")] 
        public string DuringDisasterSteps { get; set; }

        [Column("recovery_steps")]
        public string RecoverySteps { get; set; }

        public override string ToString()
        {
            return Id + " "  + DisasterType + " " + PrecautionSteps + " " + DuringDisasterSteps + " " + RecoverySteps + " " + Timestamp.ToString() + '\n';
        }
    }
}
