using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using tc.Service;

namespace tc.ViewModels
{
    public partial class SearchViewModel : ViewModelBase
    {
        private readonly ISearchableService _contentService;
        private CancellationTokenSource? _cancellationTokenSource;
        public ReactiveCommand<Unit, SearchItemViewModel?> AddContent { get; }

        private string? _searchText;
        private bool _isBusy;

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        private SearchItemViewModel? _selectedContent;

        public ObservableCollection<SearchItemViewModel> SearchResults { get; } = [];

        public SearchItemViewModel? SelectedContent
        {
            get => _selectedContent;
            set => this.RaiseAndSetIfChanged(ref _selectedContent, value);
        }

        public SearchViewModel(ISearchableService contentService)
        {
            this._contentService = contentService;

            AddContent = ReactiveCommand.Create(() =>
            {
                return SelectedContent;
            });

            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(400))
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
