using Avalonia.Media.Imaging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace tc.Service
{
    public class RestApiClient
    {
        private HttpClient httpClient;
        private HttpClientHandler handler;

        public string JSESSIONID = "";

        public HttpClient HttpClient { get { return httpClient; } }
        public HttpClientHandler Handler { get { return handler; } }

        public RestApiClient()
        {
            handler = new HttpClientHandler();
            CookieContainer cookies = new CookieContainer();
            handler.CookieContainer = cookies;
            httpClient = new HttpClient(this.handler);
            httpClient.BaseAddress = new Uri("http://localhost:80/api");
        }

        public void SetSessionCookie(string value)
        {
            handler.CookieContainer.Add(new Cookie("SESSION", value) { Domain = httpClient.BaseAddress!.Host });

        }

        public async Task<Bitmap?> GetBitmapAsync(string url)
        {
            var uri = new Uri(url);
            try
            {
                var data = await httpClient.GetByteArrayAsync(uri);
                return new Bitmap(new MemoryStream(data));
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
