using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public static class InitialApiClient
    {
        public static HttpClient Build(string token = null)
        {
            var httpClient = new HttpClient();
            if (string.IsNullOrEmpty(token))
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001");
                return httpClient;
            }
            else
            {
                httpClient.BaseAddress = new Uri("https://localhost:44390");

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer  {token}");
                return httpClient;
            }
        }
    }
}
