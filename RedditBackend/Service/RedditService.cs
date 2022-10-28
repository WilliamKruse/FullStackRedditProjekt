using Microsoft.EntityFrameworkCore;
using System.Text.Json;


using RedditAPI.Model;
using System.Linq;

namespace RedditAPI.Service;

public class RedditService
{
    private RedditContext db { get; }

    public RedditService(RedditContext db)
    {
        this.db = db;
    }



    public void SeedData()
    {
        User Uchecker = db.Users.FirstOrDefault()!;
        if (Uchecker == null)
        {
            User user1 = new User("William");
            User user2 = new User("Jesper");
            User user3 = new User("Mattias");
            db.Add(user1);
            db.Add(user2);
            db.Add(user3);
            db.SaveChanges();
            Post post1 = new Post("Hjælper vodka på tømmermænd?", "Altså jeg har det fucking dårligt og kan ikke nå min snus", db.Users.Where(u => u.Name == "William").First(), DateTime.Now);
            Post post2 = new Post("Er husholdningssprit godt mod bumser?", "Har prøvet med de der facemasks men de er lort", db.Users.Where(u => u.Name == "Jesper").First(), DateTime.Now) ;
            Post post3 = new Post("Kan man lære en kat at hente aviser?", "Min hund døde sidste uge og aviserne ligger stadig ude på fortorvet", db.Users.Where(u => u.Name == "Mattias").First(), DateTime.Now);
            db.Add(post1);
            db.Add(post2);
            db.Add(post3);
            Comment C1 = new Comment("Tror du skal finde en kæreste", 1, db.Users.Where(u => u.Name == "Mattias").First(), DateTime.Now);
            Comment C2 = new Comment("Bare du ikke drikker det, tror jeg det er okay!", 2, db.Users.Where(u => u.Name == "William").First(), DateTime.Now);
            Comment C3 = new Comment("Hey der er udskrevet valg, bare hvis du ikke har fået noget nyheder de sidste par dage", 3, db.Users.Where(u => u.Name == "Jesper").First(), DateTime.Now);
            db.Add(C1);
            db.Add(C2);
            db.Add(C3);
            db.SaveChanges();
            CommentVote(1, "Jesper", true);
            PostVote(1, "William", false);
            PostVote(1, "Jesper", false);
            PostVote(1, "Mattias", false);
            db.SaveChanges();
            PostVote(1, "William", true);
            PostVote(1, "Jesper", true);
            PostVote(1, "Mattias", true);
            db.SaveChanges();
            CreatePost("Jesperervæk","lol","hej");
            db.SaveChanges();
            CreateComment("William", "Hej Test", 4);
            
        }
        db.SaveChanges();
    }
    public List<User> GetAllUsers()
    {
        return db.Users.ToList();
    }
    public User GetUser(long id)
    {
        return db.Users.Where(u => u.UserId == id).First();
    }
    public List<Post> GetPosts()
    {
        return db.Posts.ToList();
    }
    public Post GetPost(long id)
    {
        return db.Posts.Where(p => p.PostId == id).First();
    }
    public List<Comment> GetComments(long id)
    {
        return db.Comments.Where(c => c.PostId == id).ToList();
    }
    public bool CommentVote(long commentID, string userName, bool like)
    {
        var likechecker = db.Users.Where(u => u.Name == userName).SelectMany(x => x.CommentVotes).Where(x=>x.CommentId == commentID).ToList();
       // var likechecker = db.Posts.Include(u => u.UserVotes).Where(x => x.PostId == commentID).First().UserVotes;
        if (likechecker.Count > 0)
        {
            return false;
        }
        if (!db.Users.Any(u => u.Name == userName))
        {
            User newuser = new User(userName);
            db.Add(newuser);
            db.SaveChanges();
        }
        var tempComment = db.Comments.Where(c => c.CommentId == commentID).First();
        var tempUser = db.Users.Where(u => u.Name == userName).First();
        tempComment.UserVotes.Add(tempUser);
        if (like) { tempComment.Votes++; }
        else { tempComment.Votes--; }
        db.SaveChanges();
        return true;
      
    }
    public bool PostVote(long postID, string userName, bool like)
    {
        var likechecker = db.Users.Where(u => u.Name == userName).SelectMany(x => x.PostVotes).Where(x => x.PostId == postID).ToList();
        if (likechecker.Count > 0)
        {
            return false;
        }
        if (!db.Users.Any(u => u.Name == userName))
        {
            User newuser = new User(userName);
            db.Add(newuser);
            db.SaveChanges();
        }

        var tempPost = db.Posts.Where(p => p.PostId == postID).First();
        var tempUser = db.Users.Where(u => u.Name == userName).First();
        tempPost.UserVotes.Add(tempUser);
        if (like) { tempPost.Votes++; }
        else { tempPost.Votes--; }
        db.SaveChanges();
        return true;
    }
    public Post CreatePost(string userName, string title, string body)
    {
        if (!db.Users.Any(u => u.Name == userName))
        {
            User newuser = new User(userName);
            db.Add(newuser);
            db.SaveChanges();
        }
        
        
        Post newpost = new Post(title, body, db.Users.Where(u => u.Name == userName).First(), DateTime.Now);
        db.Add(newpost);
        db.SaveChanges();
        return newpost;
    }
    public Comment CreateComment(string userName, string body, long postID)
    {

        if (!db.Users.Any(u => u.Name == userName))
        {
            User newuser = new User(userName);
            db.Add(newuser);
            db.SaveChanges();
        }
            Comment newcomment = new Comment(body, postID, db.Users.Where(u => u.Name == userName).First(), DateTime.Now);
        db.Add(newcomment);
        db.SaveChanges();
            return newcomment;

    }
}