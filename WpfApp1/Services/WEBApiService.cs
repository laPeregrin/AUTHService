using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Wraps;

namespace WpfApp1.Services
{
    public class WEBApiService
    {
        private string curToken { get; set; }
        private string curRefreshToken { get; set; }
        private string currId { get; set; }

        private HttpClient httpClient { get; set; }


        public async Task AddPost(string text)
        {
            httpClient = InitialApiClient.Build(curToken);
            var body = new UserRequest
            {
                MessageRequest = text
            };
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using var httpResponse = await httpClient.PostAsync("/api/Home/AddPost", bodyJson);

            var content = await httpResponse.Content.ReadAsStringAsync();
            if (content.StartsWith("SUCCESS!"))
                MessageBox.Show("SUCCESS Added");
        }

        public async Task RemovePostById(string id)
        {
            httpClient = InitialApiClient.Build(curToken);
            var body = new UserRequest
            {
                MessageRequest = id
            };
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using var httpResponse = await httpClient.PostAsync("/api/Home/RemovePost", bodyJson);

            var content = await httpResponse.Content.ReadAsStringAsync();
            if (content.StartsWith("SUCCESS!"))
                MessageBox.Show("SUCCESS DELETE");
        }

        public async Task<IEnumerable<Post>> GetPost(string content)
        {
            httpClient = InitialApiClient.Build(curToken);

            if (content == null)
            {
                using var httpResponse = await httpClient.GetAsync("/api/Home/GetPosts");


                return await httpResponse.Content.ReadAsAsync<IEnumerable<Post>>();
            }
            else
            {
                
                var body = new UserRequest { MessageRequest = content };
                var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                using var httpResponse = await httpClient.PostAsync("/api/Home/GetPostsByHashTag", bodyJson);

                var posts = await httpResponse.Content.ReadAsAsync<IEnumerable<Post>>();

                return posts;
            }
           
        }


        public async Task login(string login, string password)
        {
            httpClient = InitialApiClient.Build();
            var body = new LoginRequestBody
            {
                Password = password,
                UserName = login
            };
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using (var httpResponse = await httpClient.PostAsync("/API/login", bodyJson))
            {
                var content = await httpResponse.Content.ReadAsAsync<UserLoginResponce>();
                curToken = content.AccesToken;
                curRefreshToken = content.RefreshToken;
            }
        }
    }
}
