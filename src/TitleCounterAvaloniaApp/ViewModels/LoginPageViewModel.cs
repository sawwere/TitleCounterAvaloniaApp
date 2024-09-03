using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using System.Diagnostics;
using System.Net;
using System.Windows.Input;
using tc.Service;
using tc.Utils;

namespace tc.ViewModels
{
    public partial class LoginPageViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly IMessenger _messenger;
        public ICommand LoginCommand { get; }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }
        private string _username = "admin";
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        private string _password = "1111";
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public LoginPageViewModel(AuthService authService, IMessenger messenger)
        {
            _authService = authService;
            _messenger = messenger;
            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var authResult = await _authService.AuthenticateAsync(new Dto.UserLoginDto(Username, Password));
                if (authResult is null)
                {
                    ErrorMessage = "Error encountered while performing request. Check your connetion and try again later.";
                }
                else if (!authResult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    switch (authResult.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Unauthorized: ErrorMessage = "Invalid username or password"; break;
                        default: ErrorMessage = "Error encountered while performing request. Try again later."; break;
                    }
                    return;
                }
                else
                {
                    ErrorMessage = string.Empty;
                    Debug.WriteLine($"Logged as {authResult.User!.Username}");
                    _messenger.Send(new LoginSuccessMessage(authResult));
                }
            });
        }
    }
}
