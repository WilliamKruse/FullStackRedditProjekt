using Microsoft.EntityFrameworkCore;

namespace RedditAPI.Model
{
    public class RedditContext : DbContext
    {

        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();

        public string DbPath { get; }

        public RedditContext(DbContextOptions<RedditContext> options)
             : base(options)
        {
          
        }


    }
}