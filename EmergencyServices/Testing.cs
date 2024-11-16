using System;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

[assembly: InternalsVisibleTo("EmergencyServices_Group8_Tests")]
namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    [Table("testing")] // Table for testing only
    internal class Testing : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Proper
        //public DateTime CreatedAt { get => CreatedAt; set { CreatedAt = DateTime.Now; } } // Does Not Work; Stack Overflow

        [Column("test_string")]
        public string TestString { get; set; }

        public override string ToString()
        {
            return Id + " " + CreatedAt.ToString() + " " + TestString;
        }
    }
}
