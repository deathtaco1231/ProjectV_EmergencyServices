﻿using System;
using Postgrest.Models;
using Postgrest.Attributes;

namespace EmergencyServices
{
    [Table("testing")]
    internal class Testing : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime CreatedAt { get => CreatedAt; set { CreatedAt = DateTime.Now; } }

        [Column("test_string")]
        public string TestString { get; set; }

        public override string ToString()
        {
            return Id + " " + CreatedAt.ToString() + " " + TestString;
        }
    }
}
