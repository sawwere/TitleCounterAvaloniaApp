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
using tc.Utils.Exception;

namespace tc.Service
{
    public class AuthService
    {
        private readonly RestApiClient _restApiClient;
        private readonly AuthResultFactory _authenticationResultFactory;
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }

        public AuthService(RestApiClient restApiClient)
        {
            _restApiClient = restApiClient;
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            _authenticationResultFactory = new AuthResultFactory();
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
            //var content = JsonContent.Create(new KeyValuePair<string, string>("refreshToken", refreshToken));
            //HttpResponseMessage result = _restApiClient
            //    .HttpClient.PostAsync("http://localhost:80/api/auth/token", content)
            //    .Result
            //    .EnsureSuccessStatusCode();
            //_setTokens(JsonConvert.DeserializeObject<JwtAuthenticationResponse>(result.Content.ReadAsStringAsync().Result)!);
        }

        private void _setTokens(JwtAuthenticationResponse jwt)
        {
            AccessToken = jwt.accessToken;
            RefreshToken = jwt.refreshToken;
            _restApiClient.SetAuthorizationHeader(new AuthenticationHeaderValue("Bearer", jwt.accessToken));
            Debug.WriteLine(AccessToken);
        }

        public static readonly Uri LoginUri = new Uri("/api/auth/login");
        public static readonly Uri LogoutUri = new Uri("http://localhost:80/api/auth/logout");

        public void Logout()
        {
            Debug.WriteLine("Logout succesfully");
        }        

        public async Task<AuthenticationResult> AuthenticateAsync(UserLoginDto userLoginDto)
        {
            try
            {
                var response = await _restApiClient.PostJsonAsync("/api/auth/login", userLoginDto);
                if (!response.IsSuccessStatusCode)
                {
                    Exception ex;
                    if (response.StatusCode > (HttpStatusCode)399 && response.StatusCode < (HttpStatusCode)500)
                    {
                        ex = new ClientSideException();
                    }
                    else
                    {
                        ex = new ApiResponseException();
                    }
                    ex.Data["responseContent"] = response.Content;
                    return _authenticationResultFactory.MakeFailed(response.StatusCode, ex);
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
                _setTokens(jwtResponse);
                return _authenticationResultFactory.MakeSuccessfull(user, jwtResponse);
            }
            catch (HttpRequestException httpEx) 
            { 
                return _authenticationResultFactory.MakeFailed(HttpStatusCode.ServiceUnavailable, new ServiceUnavailableException(httpEx.Message));
            }
        }
    }

    internal class AuthResultFactory
    {
        public AuthenticationResult MakeSuccessfull(User user, JwtAuthenticationResponse jwtResponse) =>
            new AuthenticationResult(user, jwtResponse);

        public AuthenticationResult MakeFailed(HttpStatusCode statusCode, Exception exception)
        {
            return new AuthenticationResult(statusCode, exception);
        }
            
    }

    public class AuthenticationResult
    {
        public AuthenticationResult(HttpStatusCode statusCode, Exception exception)
        {
            StatusCode = statusCode;
            Exception = exception;
        }

        public AuthenticationResult(User user, JwtAuthenticationResponse jwtAuthenticationResponse)
        {
            User = user;
            Response = jwtAuthenticationResponse;
            StatusCode = HttpStatusCode.OK;
        }

        public Exception? Exception { get; private set; }
        public User? User { get; set; }
        public JwtAuthenticationResponse? Response { get; private set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccesfull {  get => StatusCode == HttpStatusCode.OK; }
    }
}
