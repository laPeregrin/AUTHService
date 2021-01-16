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


        private HttpClient httpClient { get; set; }


        public async Task AddPost(string text)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44390");
            if (curToken == null)
            {
                MessageBox.Show("Залогиньтесь");
                return;
            }
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer  {curToken}");
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
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44390");

            if (curToken == null)
            {
                MessageBox.Show("Залогиньтесь");
                return;
            }
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer  {curToken}");

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
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44390");

            if (curToken == null)
                MessageBox.Show("Залогиньтесь");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer  {curToken}");

            if (content == null)
            {
                using var httpResponse = await httpClient.GetAsync("/api/Home/GetPosts");


                return await httpResponse.Content.ReadAsAsync<IEnumerable<Post>>();
            }
            else
            {
                using var httpResponse = await httpClient.GetAsync("/api/Home/GetPostsByHashTag");
                var body = new UserRequest { MessageRequest = content };
                var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                using var httpResponses = await httpClient.GetAsync("/api/Home/GetPosts");

                httpClient.Dispose();

                return await httpResponses.Content.ReadAsAsync<IEnumerable<Post>>();
            }
           
        }


        public async Task login(string login, string password)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001");
            var body = new LoginRequestBody
            {
                Password = password,
                UserName = login
            };
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using var httpResponse = await httpClient.PostAsync("/API/login", bodyJson);

            var content = await httpResponse.Content.ReadAsAsync<UserLoginResponce>();
            curToken = content.AccesToken;
            curRefreshToken = content.RefreshToken;
        }
    }
}
