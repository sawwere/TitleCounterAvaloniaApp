using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using tc.Dto;
using tc.Models;

namespace tc.Service
{
    public class AuthService
    {
        private readonly RestApiClient _restApiClient;
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }

        public AuthService(RestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
        }

        public async Task RefreshTokenPeriodicallyAsync(string refreshToken, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _refreshToken(refreshToken);
                await Task.Delay(period, cancellationToken);
            }
        }

        private void _refreshToken(string refreshToken)
        {
            var content = JsonContent.Create(new KeyValuePair<string, string>("refreshToken", refreshToken));
            HttpResponseMessage result = _restApiClient
                .HttpClient.PostAsync("http://localhost:80/api/auth/token", content)
                .Result
                .EnsureSuccessStatusCode();
            _setTokens(JsonConvert.DeserializeObject<JwtAuthenticationResponse>(result.Content.ReadAsStringAsync().Result)!);
        }

        private void _setTokens(JwtAuthenticationResponse jwt)
        {
            AccessToken = jwt.accessToken;
            RefreshToken = jwt.refreshToken;
            _restApiClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.accessToken);
            Debug.WriteLine(AccessToken);
        }

        public static readonly Uri LoginUri = new Uri("http://localhost:80/api/auth/login");
        public static readonly Uri LogoutUri = new Uri("http://localhost:80/api/auth/logout");

        public void Logout()
        {
            var result = _restApiClient.HttpClient.GetAsync(LogoutUri).Result;
            result.EnsureSuccessStatusCode();
            Debug.WriteLine("Logout succesfully");
        }        

        public async Task<AuthenticationResult?> AuthenticateAsync(UserLoginDto userLoginDto)
        {
            var jsonContent = JsonContent.Create(userLoginDto);
            var response = await _restApiClient.HttpClient.PostAsync(LoginUri, jsonContent);
            if (response is null)
            {
                return null;
            }
            else if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationResult(response.StatusCode);
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var jwtResponse = JsonConvert.DeserializeObject<JwtAuthenticationResponse>(responseContent)!;
            var access = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(jwtResponse.accessToken);
            var claims = access.Claims;
            var user = new User
            {
                Id = (int)access.Payload["id"],
                Username = access.Payload.Sub,
                Email = access.Payload["email"].ToString()!,
                Roles = access.Claims.Where(x => x.Type == "roles").Select(x => x.Value).ToList()
            };
            return new AuthenticationResult(user, jwtResponse.accessToken, jwtResponse.refreshToken, jwtResponse.expiresIn, response.StatusCode);
        }
    }

    public class AuthenticationResult
    {
        public AuthenticationResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public AuthenticationResult(User user, string accessToken, string refreshToken, int expiresIn, HttpStatusCode statusCode)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            StatusCode = statusCode;
        }

        public User? User { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int? ExpiresIn { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
