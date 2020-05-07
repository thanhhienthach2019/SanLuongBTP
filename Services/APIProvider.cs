using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Services
{
    public class APIProvider
    {
        private string uri = "http://192.1.1.13:100/api/";

        public async Task<T> WebAPIGetAsync<T>(string route) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(route);
                T t;
                if (response.IsSuccessStatusCode)
                {
                    t = await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    t = null;
                }
                return t;
            }
        }

        public async Task<HttpStatusCode> WebAPIPostAsync<T>(string route, T t) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(uri);
                HttpResponseMessage response = await client.PostAsJsonAsync(route, t);
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> WebAPIPutAsync<T>(string route, T t) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(uri);
                HttpResponseMessage response = await client.PutAsJsonAsync(route, t);
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> WebAPIDeleteAsync(string route)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(uri);
                HttpResponseMessage response = await client.DeleteAsync(route);
                return response.StatusCode;
            }
        }
    }
}