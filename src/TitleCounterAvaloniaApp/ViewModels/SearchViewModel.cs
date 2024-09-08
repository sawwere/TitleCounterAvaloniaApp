using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using tc.Models;
using tc.Service;

namespace tc.ViewModels
{
    public partial class SearchViewModel : ViewModelBase
    {
        private readonly ISearchableService _contentService;
        private readonly IMessenger _messenger;
        private CancellationTokenSource? _cancellationTokenSource;
        public ReactiveCommand<Unit, Entry?> AddContent { get; }

        private string? _searchText;
        //[ObservableProperty]
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private SearchItemViewModel? _selectedContent;

        public ObservableCollection<SearchItemViewModel> SearchResults { get; } = [];

        public SearchItemViewModel? SelectedContent
        {
            get => _selectedContent;
            set => this.RaiseAndSetIfChanged(ref _selectedContent, value);
        }

        public SearchViewModel(ISearchableService contentService, IMessenger messenger)
        {
            _contentService = contentService;
            _messenger = messenger;
            IsBusy = false;
            AddContent = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedContent is null)
                    return null;
                IsBusy = true;
                var res = await _contentService.Create(SelectedContent.Item);
                if (res is not null)
                {
                    //IsBusy = false;
                    _messenger.Send(res);
                }
                else
                {
                    //TODO Error handling
                }
                IsBusy = false;
                return res;
            });

            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch!);
        }

        private async void DoSearch(string? s)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            IsBusy = true;
            SearchResults.Clear();
            if (!string.IsNullOrWhiteSpace(s))
            {

                var results = _contentService.SearchByTitle(s);

                foreach (var content in results)
                {
                    var vm = new SearchItemViewModel(content);
                    SearchResults.Add(vm);
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    LoadCovers(cancellationToken);
                }
            }
            IsBusy = false;
        }

        private async void LoadCovers(CancellationToken cancellationToken)
        {
            foreach (var content in SearchResults.ToList())
            {
                await content.LoadCover();

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}
