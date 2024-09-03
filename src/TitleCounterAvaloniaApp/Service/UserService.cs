using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using tc.Dto;
using tc.Models;
using tc.Utils;
using tc.Utils.Exception;

namespace tc.Service
{
    public class UserService
    {
        private readonly IMessenger _messenger;
        private readonly RestApiClient _restClient;
        public UserService(IMessenger messenger, RestApiClient restApiClient)
        {
            _messenger = messenger;
            _restClient = restApiClient;
            _messenger.Register<UserService, LoginSuccessMessage>(this, (_, message) =>
            {
                OnUserLogin(message.Value.User!);
            });
        }

        public User GetCurrentUserOrThrow()
        {
            if (!IsLoggedIn())
            {
                throw new InstanceNotPresentException("Value of CurrentUser is null");
            }
            return CurrentUser!;
        }

        public User? CurrentUser { get; private set; }

        public bool IsLoggedIn() => CurrentUser != null;

        public void OnUserLogin(User user)
        {
            CurrentUser = user;
        }


        public void OnUserLogout(User user)
        {
            Debug.WriteLine("LOGOUT ");
            CurrentUser = null;
        }

        public async Task<UserDto?> RefreshCurrentUserAsync()
        {
            var response = _restClient.HttpClient.GetAsync("http://localhost:80/api/user");
            var responseContent = await response.Result.Content.ReadAsStringAsync();
            if (!response.Result.IsSuccessStatusCode)
            {
                return null;
            }
            var userDto = JsonConvert.DeserializeObject<UserDto>(responseContent)!;
            return userDto;
        }
    }
}
