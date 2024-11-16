using Postgrest.Models;
using Postgrest.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    [Table("test_disaster_processed")] //Target the test table
    public class TestProcessedDisaster : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("disaster_type")]
        public string DisasterType { get; set; }

        [Column("priority")]
        public string Priority { get; set; }  //Watch, Warning, Urgent, Critical

        [Column("alert_description")]
        public string Description { get; set; }

        [Column("precaution_steps")]
        public string PrecautionSteps { get; set; }

        [Column("during_disaster_steps")]
        public string DuringDisasterSteps { get; set; }

        [Column("recovery_steps")]
        public string RecoverySteps { get; set; }

        [Column("created_at")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Column("severity_level")]
        public double SeverityLevel { get; set; }  // E.g., Rainfall in mm, Hurricane category

        [Column("notif_source")]
        public string Source { get; set; }  // NWS or Emergency Services

        public override string ToString()
        {
            return Id + " " + DisasterType.ToString() + " " + Priority + " " + Description + " " + PrecautionSteps + " " + DuringDisasterSteps + " " + RecoverySteps + " " + Timestamp.ToString();
        }
    }
}

