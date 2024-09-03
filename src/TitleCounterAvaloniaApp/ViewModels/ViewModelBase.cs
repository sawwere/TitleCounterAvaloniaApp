using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System.ComponentModel;

namespace tc.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IReactiveObject
    {
        public void RaisePropertyChanging(PropertyChangingEventArgs args) => OnPropertyChanging(args);

        public void RaisePropertyChanged(PropertyChangedEventArgs args) => OnPropertyChanged(args);
    }
}
