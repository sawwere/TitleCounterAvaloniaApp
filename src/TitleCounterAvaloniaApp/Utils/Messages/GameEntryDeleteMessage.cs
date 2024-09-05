using CommunityToolkit.Mvvm.Messaging.Messages;
using tc.ViewModels.EntryViewModels;

namespace tc.Utils.Messages
{
    public class GameEntryDeleteMessage: ValueChangedMessage<GameEntryViewModel>
    {
        public GameEntryDeleteMessage(GameEntryViewModel value) : base(value)
        {
        }
    }
}
