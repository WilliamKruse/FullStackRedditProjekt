using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using RedditAPI.Service;
using RedditAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<RedditContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<RedditService>();

// Dette kode kan bruges til at fjerne "cykler" i JSON objekterne.

builder.Services.Configure<JsonOptions>(options =>
{
    // Her kan man fjerne fejl der opstår, når man returnerer JSON med objekter,
    // der refererer til hinanden i en cykel.
    // (altså dobbelrettede associeringer)
    options.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

// Seed data hvis nødvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<RedditService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

app.MapGet("/getallpost", (RedditService service) =>
{
    return service.GetPosts();
});
app.MapGet("/getallusers", (RedditService service) =>
{
    return service.GetAllUsers();
});

app.MapGet("/getpost/{id}", (RedditService service, long id) =>
{
    return service.GetPost(id);
});
app.MapGet("/getuser/{id}", (RedditService service, long id) =>
{
    return service.GetUser(id);
});
app.MapGet("/getallcomment/{id}", (RedditService service, long id) =>
{
    return service.GetComments(id);
});

app.MapPost("/post/vote", (RedditService service, VoteDTO data) =>
 {
     return service.PostVote(data.voteID, data.userName, data.like);

 });
app.MapPost("/comment/vote", (RedditService service, VoteDTO data) =>
{
    return service.CommentVote(data.voteID, data.userName, data.like);

});
app.MapPost("/createpost", (RedditService service, PostDTO data) =>
 {
     return service.CreatePost(data.userName, data.title, data.body);
 });
app.MapPost("/createcomment", (RedditService service, CommentDTO data) =>
{
    return service.CreateComment(data.userName, data.body, data.postID);
});

// DataService fås via "Dependency Injection" (DI)

app.Run();
record VoteDTO(long voteID,string userName,bool like);
record PostDTO(string userName, string title, string body);
record CommentDTO(string userName, string body, long postID );