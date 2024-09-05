using Avalonia.Media.Imaging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace tc.Service
{
    public class RestApiClient
    {
        private HttpClient _httpClient;
        private HttpClientHandler _handler;

        public string SessionCookie = "";

        //public HttpClient HttpClient { get { return _httpClient; } }

        public RestApiClient()
        {
            _handler = new HttpClientHandler();
            CookieContainer cookies = new CookieContainer();
            _handler.CookieContainer = cookies;
            _httpClient = new HttpClient(this._handler);
            _httpClient.BaseAddress = new Uri("http://localhost:80");
        }

        public Task<TValue?> GetFromJsonAsync<TValue>(string uri)
        {
            var res= _httpClient.GetFromJsonAsync<TValue>(uri);
            return res;
        }

        public Task<HttpResponseMessage> PostJsonAsync<TValue>(string uri, TValue value)
        {
            return _httpClient.PostAsJsonAsync<TValue>(uri, value);
        }

        public Task<HttpResponseMessage> PutJsonAsync<TValue>(string uri, TValue value)
        {
            return _httpClient.PutAsJsonAsync<TValue>(uri, value);
        }

        public Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return _httpClient.DeleteAsync(uri);
        }

        public Task<byte[]> GetByteArrayAsync(string uri)
        {
            return _httpClient.GetByteArrayAsync(uri);
        }

        public void SetAuthorizationHeader(AuthenticationHeaderValue value)
        {
            _httpClient.DefaultRequestHeaders.Authorization = value;
            //_handler.CookieContainer.Add(new Cookie("SESSION", value) { Domain = _httpClient.BaseAddress!.Host });

        }
    }
}
