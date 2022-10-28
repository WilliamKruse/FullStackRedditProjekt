using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditAPI.Model
{
    public class Comment
    {
        public long CommentId { get; set; }
        public long PostId { get; set; }
        public string Body { get; set; }
        public int Votes { get; set; }
        public long UserId { get; set; }
        public DateTime CommentTime { get; set; }
        public virtual ICollection<User> UserVotes { get; set; } = new List<User>();

        public Comment(string body, long postID, User user, DateTime commentTime)
        {
            
            this.Body = body;
            this.PostId = postID;
            this.Votes = 0;
            this.UserId = user.UserId;
            this.CommentTime = commentTime;
            this.UserVotes = new HashSet<User>();
        }
        public Comment()
        {

        }
    }
}
