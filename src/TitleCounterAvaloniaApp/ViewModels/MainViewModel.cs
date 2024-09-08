using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using tc.Service;
using tc.Utils;

namespace tc.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly UserService _userService;

        //[ObservableProperty]
        private bool _isPaneVisible;
        public bool IsPaneVisible
        {
            get => _isPaneVisible;
            set => this.RaiseAndSetIfChanged(ref _isPaneVisible, value);
        }

        //[ObservableProperty]
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        //[ObservableProperty]
        private ViewModelBase _currentPage = Ioc.Default.GetRequiredService<LoginPageViewModel>();
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public ObservableCollection<ListItemTemplate> Items { get; } =
        [
            new ListItemTemplate(typeof(HomePageViewModel), "home_regular"),
        ];

        //[ObservableProperty]
        private ListItemTemplate _selectedListItem;
        public ListItemTemplate SelectedListItem
        {
            get => _selectedListItem;
            set => this.RaiseAndSetIfChanged(ref _selectedListItem, value);
        }

        private readonly IMessenger _messenger;

        public Interaction<SearchViewModel, SearchItemViewModel?> ShowDialog { get; }

        public MainViewModel(UserService userService, IMessenger messenger)
        {
            _userService = userService;
            _messenger = messenger;
            _messenger.Register<MainViewModel, LoginSuccessMessage>(this, (_, message) =>
            {
                IsPaneVisible = true;
                SelectedListItem = Items[0];
                Items.Add(new ListItemTemplate(typeof(GamesPageViewModel), "games_regular"));
                Items.Add(new ListItemTemplate(typeof(FilmsPageViewModel), "movies_and_tv_regular"));
            });

            ShowDialog = new Interaction<SearchViewModel, SearchItemViewModel?>();
            this.WhenPropertyChanged(_ => _.SelectedListItem).Subscribe(x =>
            {
                if (x.Value is null)
                    return;
                var instance = Design.IsDesignMode
                    ? Activator.CreateInstance(x.Value.ModelType)
                    : Ioc.Default.GetService(x.Value.ModelType);
                if (instance is null)
                    return;
                CurrentPage = (ViewModelBase)instance;
            });


            
        }

        public MainViewModel() : this(Ioc.Default.GetRequiredService<UserService>(), new WeakReferenceMessenger())
        {
        }
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey)
        {
            this.ModelType = type;
            Label = Assets.Resources.ResourceManager.GetString("PaneItem_" + type.Name.Replace("ViewModel",""), Assets.Resources.Culture)!;
            //Label = type.Name.Replace("PageViewModel", "");

            Application.Current!.TryFindResource(iconKey, out var res);
            ListItemIcon = (StreamGeometry)res!;
        }

        public string Label { get; }
        public Type ModelType { get; }
        public StreamGeometry ListItemIcon { get; }
    }
}


