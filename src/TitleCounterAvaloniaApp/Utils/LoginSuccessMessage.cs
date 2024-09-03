using CommunityToolkit.Mvvm.Messaging.Messages;
using tc.Service;

namespace tc.Utils
{
    public class LoginSuccessMessage : ValueChangedMessage<AuthenticationResult>
    {
        public LoginSuccessMessage(AuthenticationResult value) : base(value)
        {
        }
    }
}
