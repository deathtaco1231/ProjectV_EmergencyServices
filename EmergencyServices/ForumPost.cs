using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Models;
using Postgrest.Attributes;
using Supabase.Core.Attributes;

namespace EmergencyServices.Group8
{
    [Table("forum_posts")]
    public class ForumPost : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime createdAt { get; set; }

        [Column("user_name")]
        public string userName { get; set; }

        [Column("post_header")]
        public string postHeader { get; set; }

        [Column("post_body")]
        public string postBody { get; set; }

        public override string ToString()
        {
            return Id.ToString() + "," + createdAt.ToString() + "," + userName + "," + postHeader + "," + postBody;
        }

        public void CreatePost(string name, string postHead, string body)
        {

        }
    }
}
