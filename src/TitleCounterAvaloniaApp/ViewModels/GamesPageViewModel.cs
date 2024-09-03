using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using tc.Models;
using tc.Service;
using tc.ViewModels.EntryViewModels;

namespace tc.ViewModels
{
    public partial class GamesPageViewModel : ViewModelBase
    {
        private readonly GameService _service;
        public ObservableCollection<GameEntryViewModel> Contents { get; } = [];
        [ObservableProperty]
        private GameEntryViewModel _selectedEntry;
        public ICommand SearchCommand { get; }
        public Interaction<SearchViewModel, SearchItemViewModel?> ShowDialog { get; }

        public GamesPageViewModel(GameService service)
        {
            _service = service;
            LoadContents();

            SearchCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var store = new SearchViewModel(_service);

                var result = await ShowDialog.Handle(store);
                if (result != null)
                {
                    var responseDto = await _service.Create(result.Item);
                    if (responseDto is not null)
                    {
                        Contents.Add(new GameEntryViewModel(GameService.DtoToEntry(responseDto)));
                    }
                    //TODO 
                }
            });
        }

        private async void LoadContents()
        {
            IEnumerable<GameEntryViewModel> contents;
            contents = (await _service.Load()).Select(x => new GameEntryViewModel(x as GameEntry));
            
            Contents.Clear();
            Contents.AddRange(contents);
            LoadCovers(CancellationToken.None);
            Debug.WriteLine($"Contents size: {Contents.Count}");
        }

        private async void LoadCovers(CancellationToken cancellationToken)
        {
            foreach (var content in Contents.ToList())
            {
                await content.LoadCover();
            }
        }
    }
}
