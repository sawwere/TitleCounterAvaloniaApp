using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using DynamicData.Binding;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using tc.Models;
using tc.Service;
using tc.Utils;
using tc.Utils.Exception;
using tc.Utils.Messages;
using tc.ViewModels.EntryViewModels;

namespace tc.ViewModels
{
    public partial class GamesPageViewModel : ViewModelBase
    {
        private readonly GameService _service;
        private readonly IMessenger _messenger;
        [ObservableProperty]
        private SearchViewModel _search;
        private readonly ReadOnlyObservableCollection<GameEntryViewModel> _contents;
        public ReadOnlyObservableCollection<GameEntryViewModel> TestViewModels => _contents;
        //public ObservableCollection<GameEntryViewModel> Contents { get; } = [];

        [ObservableProperty]
        private GameEntryViewModel _selectedEntry;
        [ObservableProperty]
        private ErrorMessageOverlayViewModel _errorOverlay = new ErrorMessageOverlayViewModel("");

        public GamesPageViewModel(GameService service, IMessenger messenger)
        {
            _service = service;
            _messenger = messenger;
            _search = new SearchViewModel(service, messenger);
            LoadContents();
            _sourceCache.Connect()
       .Filter(x => x.S.Equals("backlog"))
       // Bind to our ReadOnlyObservableCollection<T>
       .Bind(out _contents)
       // Subscribe for changes
       .Subscribe();

            _messenger.Register<GamesPageViewModel, GameEntryDeleteMessage>(this, (_, message) =>
            {
                //bool res = Contents.Remove(message.Value);
                //if (res)
                //{
                //    service.Remove(message.Value.Entry.Id);
                //    Debug.WriteLine(message.Value.Entry.Id);
                //    SelectedEntry = null!;
                //}
            });
            _messenger.Register<GamesPageViewModel, GameEntry>(this, async (_, message) =>
            {
                //var newEntryViewModel = new GameEntryViewModel(_service, message);
                //Contents.Add(newEntryViewModel);
                //await newEntryViewModel.LoadCover();
                //Debug.WriteLine(message.Id);
            });

            
        }
        private SourceCache<GameEntryViewModel, long> _sourceCache = new(x => x.Entry.Id);
        private async void LoadContents()
        {
            try
            {
                IEnumerable<GameEntryViewModel> contents = (await _service.FindAll()).Select(x => new GameEntryViewModel(_service, (x as GameEntry)!));
                foreach (var content in contents)
                {
                    _sourceCache.AddOrUpdate(content);
                }
                //TestViewModels.AddRange(contents);
                //Contents.Clear();
                //Contents.AddRange(contents);
                LoadCovers(CancellationToken.None);
                ErrorOverlay.Message = string.Empty;

               
            }
            catch (ServiceUnavailableException ex)
            {
                ErrorOverlay = new ErrorMessageOverlayViewModel(Assets.Resources.ServiceUnavableMessage);
                Debug.WriteLine(ex.Message);
            }
        }

        private async void LoadCovers(CancellationToken cancellationToken)
        {
            //foreach (var content in Contents.ToList())
            //{
            //    await content.LoadCover();
            //}
            foreach (var content in TestViewModels.ToList())
            {
                await content.LoadCover();
            }
        }
    }
}
