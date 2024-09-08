using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tc.ViewModels
{
    public partial class ErrorMessageOverlayViewModel : ViewModelBase
    {
        //[ObservableProperty]
        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref this._message, value);
        }
        private static readonly string DEFAULT_MESSAGE = Assets.Resources.ServiceUnavableMessage;

        public ErrorMessageOverlayViewModel()
        {
            _message = DEFAULT_MESSAGE;
        }

        public ErrorMessageOverlayViewModel(string message)
        {
            _message = DEFAULT_MESSAGE + Environment.NewLine + message;
        }
    }
}
