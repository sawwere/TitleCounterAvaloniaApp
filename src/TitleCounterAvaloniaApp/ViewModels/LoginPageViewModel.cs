using CommunityToolkit.Mvvm.ComponentModel;
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
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage = "";
        [ObservableProperty]
        private string _username = "admin";
        [ObservableProperty]
        private string _password = "1111";

        public LoginPageViewModel(AuthService authService, IMessenger messenger)
        {
            _authService = authService;
            _messenger = messenger;
            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                IsBusy = true;
                var authResult = await _authService.AuthenticateAsync(new Dto.UserLoginDto(Username, Password));
                IsBusy = false;
                if (!authResult.IsSuccesfull)
                {
                    switch (authResult.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Unauthorized: ErrorMessage = Assets.Resources.LoginPage_InvalidUsernamePassword; break;
                        default: ErrorMessage = Assets.Resources.ServiceUnavableMessage; break;
                    }
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
