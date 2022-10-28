using System;

namespace RedditAPI.Model
{
    public class User
    {
        public long UserId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> PostVotes { get; set; } = new List<Post>();
        public virtual ICollection<Comment> CommentVotes { get; set; } = new List<Comment>();

        public User(string name) 
        {
            this.Name = name;
            this.PostVotes = new HashSet<Post>();
            this.CommentVotes = new HashSet<Comment>();
        }
        public User()
        {

        }
    }
}
