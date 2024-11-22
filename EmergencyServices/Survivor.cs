using System;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Diagnostics.CodeAnalysis;
// CURRENTLY UNUSED
namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    [Table("survivor_information")]
    public class Survivor_Information : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime Date { get; set; }

        [Column("survivor_name")]
        public string SurvivorName { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("priority")]
        public string Priority { get; set; }
    }
}
