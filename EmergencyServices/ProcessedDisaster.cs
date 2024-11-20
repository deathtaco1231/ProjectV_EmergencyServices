using Postgrest.Models;
using Postgrest.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    [Table("disaster_processed")]
    public class ProcessedDisaster : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("disaster_type")]
        public string DisasterType { get; set; }

        [Column("priority")]
        public string Priority { get; set; }  // Watch, Warning, Urgent, Critical

        [Column("alert_description")]
        public string Description { get; set; }

        [Column("precaution_steps")] // FIRST ADDITIONAL FIELD NOT IN STANDARD ALERT
        public string PreparationSteps { get; set; }

        [Column("during_disaster_steps")] // SECOND ADDITIONAL FIELD
        public string ActiveSteps { get; set; }

        [Column("recovery_steps")]
        public string RecoverySteps { get; set; }

        [Column("created_at")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Column("severity_level")]
        public double SeverityLevel { get; set; }  // E.g., Rainfall in mm, Hurricane category

        [Column("notif_source")]
        public string Source { get; set; }  // NWS or Emergency Services

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Id + " " + DisasterType + " " + Priority + " " + Description + " " + PreparationSteps + " " + ActiveSteps + " " + RecoverySteps + " " + Timestamp.ToString() + " " + SeverityLevel + " " + Source + '\n';
        }
    }
}
