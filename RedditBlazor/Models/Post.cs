using System;
namespace RedditAPI.Model
{
    public class Post
    {
        public long PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Votes { get; set; }
        public long UserID { get; set; }
        public DateTime PostTime { get; set; }
        public virtual ICollection<User> UserVotes { get; set; } = new List<User>();

        public Post(string title, string body, User user, DateTime postTime)
        {
            this.Title = title;
            this.Body = body;
            this.Votes = 0;
            this.UserID = user.UserId;
            this.PostTime = postTime;
            this.UserVotes = new HashSet<User>();
        }
        public Post()
        {
            ;
        }
    }
}
