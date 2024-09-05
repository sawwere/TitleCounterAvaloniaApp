using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tc.ViewModels
{
    public partial class ErrorMessageOverlayViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _message;

        public ErrorMessageOverlayViewModel(string message)
        {
            _message = message;
        }
    }
}
