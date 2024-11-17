using System;
using Postgrest.Models;
using Postgrest.Attributes;
using System.Diagnostics.CodeAnalysis;
// CURRENTLY UNUSED
namespace EmergencyServices.Group8
{
    [ExcludeFromCodeCoverage]
    [Table("forum_posts")]
    public class ForumPost : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime CreatedAt { get => createdAt; set { createdAt = DateTime.Now; } }

        [Column("user_name")]
        public string userName { get; set; }

        [Column("post_header")]
        public string postHeader { get; set; }

        [Column("post_body")]
        public string postBody { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Id.ToString() + "," + CreatedAt.ToString() + "," + userName + "," + postHeader + "," + postBody;
        }

        [ExcludeFromCodeCoverage]
        public void CreatePost(string name, string postHead, string body)
        {

        }
    }
}
