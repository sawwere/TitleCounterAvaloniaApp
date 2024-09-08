using CommunityToolkit.Mvvm.Messaging.Messages;
using tc.ViewModels.Game;

namespace tc.Utils.Messages
{
    public class GameEntryDeleteMessage: ValueChangedMessage<GameEntryViewModel>
    {
        public GameEntryDeleteMessage(GameEntryViewModel value) : base(value)
        {
        }
    }
}
