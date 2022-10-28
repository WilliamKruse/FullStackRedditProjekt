using System;
using System.Net.Http.Json;
using RedditAPI.Model;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace RedditClientSide.Service
{
    public class RedditService
    {

        private readonly HttpClient http;
        private readonly IConfiguration configuration;
        private readonly string baseAPI = "";

        public RedditService(HttpClient http, IConfiguration configuration)
        {
            this.http = http;
            this.configuration = configuration;
            this.baseAPI = configuration["base_api"];
        }

        public async Task<User[]> GetAllUsers()
        {
            string url = $"{baseAPI}getallusers";
            return await http.GetFromJsonAsync<User[]>(url);
        }
        public async Task<User> GetUser(long id)
        {
            string url = $"{baseAPI}getuser/{id}";
            return await http.GetFromJsonAsync<User>(url);
        }


        public async Task<Post[]> GetAllPosts()
        {
            string url = $"{baseAPI}getallpost";
            return await http.GetFromJsonAsync<Post[]>(url);
        }
        public async Task<Post> GetPost(long id)
        {
            string url = $"{baseAPI}getpost/{id}";
            return await http.GetFromJsonAsync<Post>(url);
        }
        public async Task<Comment[]> GetAllComments(long id)
        {
            string url = $"{baseAPI}getallcomment/{id}";
            return await http.GetFromJsonAsync<Comment[]>(url);
        }
        public async Task<Post> CreatePost(PostDTO data)
        {
            string url = $"{baseAPI}createpost";
            HttpResponseMessage response = await http.PostAsJsonAsync(url, data);
            string jsonser = response.Content.ReadAsStringAsync().Result;
            Post? newPost = JsonSerializer.Deserialize<Post>(jsonser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return newPost;
        }
        public async Task<Comment> CreateComment(CommentDTO data)
        {
            string url = $"{baseAPI}createcomment";
            HttpResponseMessage response = await http.PostAsJsonAsync(url, data);
            string jsonser = response.Content.ReadAsStringAsync().Result;
            Comment? newComment = JsonSerializer.Deserialize<Comment>(jsonser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return newComment;
        }
        public async Task<bool> PostVote(VoteDTO data)
        {
            string url = $"{baseAPI}post/vote";
            HttpResponseMessage response = await http.PostAsJsonAsync(url, data);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            string jsonser = response.Content.ReadAsStringAsync().Result;
            bool newComment = JsonSerializer.Deserialize<bool>(jsonser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return newComment;
        }
        public async Task<bool> CommentVote(VoteDTO data)
        {
            string url = $"{baseAPI}comment/vote";
            HttpResponseMessage response = await http.PostAsJsonAsync(url, data);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            string jsonser = response.Content.ReadAsStringAsync().Result;
            bool newComment = JsonSerializer.Deserialize<bool>(jsonser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return newComment;
        }
    }
    public record VoteDTO(long voteID, string userName, bool like);
    public record PostDTO(string userName, string title, string body);
    public record CommentDTO(string userName, string body, long postID);
}