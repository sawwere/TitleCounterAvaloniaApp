using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using DynamicData.Binding;
using DynamicData.PLinq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using tc.Models;
using tc.Service;
using tc.Utils.Exception;
using tc.Utils.Messages;
using tc.ViewModels.Game;

namespace tc.ViewModels
{
    public partial class GamesPageViewModel : ViewModelBase
    {
        private readonly GameService _service;
        private readonly IMessenger _messenger;

        private SearchViewModel _search;
        public SearchViewModel Search
        {
            get => _search;
            set => this.RaiseAndSetIfChanged(ref _search, value);
        }

        private GameChartViewModel _charts;
        public GameChartViewModel Charts
        {
            get => _charts;
            set => this.RaiseAndSetIfChanged(ref _charts, value);
        }

        private readonly ReadOnlyObservableCollection<GameEntryViewModel> _contents;
        public ReadOnlyObservableCollection<GameEntryViewModel> TestViewModels => _contents;

        private GameEntryViewModel _selectedEntry;
        public GameEntryViewModel SelectedEntry
        {
            get => _selectedEntry;
            set { this.RaiseAndSetIfChanged(ref _selectedEntry, value); OverlayOpacity = 0.9; }
        }

        private ErrorMessageOverlayViewModel _errorOverlay = new ErrorMessageOverlayViewModel("");
        public ErrorMessageOverlayViewModel ErrorOverlay
        {
            get { return _errorOverlay; }
            set => this.RaiseAndSetIfChanged(ref _errorOverlay, value);
        }

        public List<string> statusFilterItems
        {
            get => Models.Status.LocalizedStatuses.ToList();
        }

        private static List<string> _scoreFilterItems = [Assets.Resources.NoScore_Label, "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];
        public List<string> ScoreFilterItems
        {
            get => _scoreFilterItems;
        }
        private ObservableCollection<string> _releaseYearsFilterItems = [Assets.Resources.NoDateRelease_Label];
        public ObservableCollection<string> ReleaseYearsFilterItems
        {
            get => _releaseYearsFilterItems;
        }

        private ObservableCollection<string> _completitionYearsFilterItems = [Assets.Resources.NoDateCompleted_Label];
        public ObservableCollection<string> CompletitionYearsFilterItems
        {
            get => _completitionYearsFilterItems;
        }
        private double _overlayOpacity = 0;
        public double OverlayOpacity
        {
            get => _overlayOpacity;
            set => this.RaiseAndSetIfChanged(ref _overlayOpacity, value);
        }
        private int? _selectedScoreFilterIndex;
        public int? SelectedScoreFilterIndex
        {
            get => _selectedScoreFilterIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedScoreFilterIndex, value);
        }
        private int? _selectedStatusFilterIndex;
        public int? SelectedStatusFilterIndex
        {
            get => _selectedStatusFilterIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedStatusFilterIndex, value);
        }
        private int? _selectedReleaseYearFilterIndex;
        public int? SelectedReleaseYearFilterIndex
        {
            get => _selectedReleaseYearFilterIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedReleaseYearFilterIndex, value);
        }
        private int? _selectedCompletitionYearFilterIndex;
        public int? SelectedCompletitionYearFilterIndex
        {
            get => _selectedCompletitionYearFilterIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedCompletitionYearFilterIndex, value);
        }
        private int _sortingSelectedIndex = 0;
        public int SortingSelectedIndex
        {
            get => _sortingSelectedIndex;
            set => this.RaiseAndSetIfChanged(ref _sortingSelectedIndex, value);
        }

        private Func<GameEntryViewModel, bool> statusFilter(int? index) => entry =>
        {
            //no selectedIndex => no filtering
            if (index is null || index == -1) return true;
            // status is string AND entry always has non-Null status
            string status = Models.Status.Statuses[index.Value];
            return entry.Status == status;
        };
        private Func<GameEntryViewModel, bool> scoreFilter(int? index) => entry =>
        {
            //no selectedIndex => no filtering
            if (index is null || index == -1) return true;
            //selectedIndex exists => item is "NoScore" or int
            if (_scoreFilterItems[index.Value] == Assets.Resources.NoScore_Label)
                return !entry.Score.HasValue;
            // score is integer, check if entry has null score
            if (!entry.Score.HasValue) return false;
            // score is integer AND entry has non-Null score
            int intScore = int.Parse(_scoreFilterItems[index.Value]);
            return entry.Score.Value == (intScore);
        };
        private Func<GameEntryViewModel, bool> releaseYearFilter(int? index) => entry =>
        {
            //no selectedIndex => no filtering
            if (index is null || index == -1) return true;
            //selectedIndex exists => item is "NoReleaseDate" or int
            if (_releaseYearsFilterItems[index.Value] == Assets.Resources.NoDateRelease_Label)
                return !entry.DateRelease.HasValue;
            // year is integer, check if entry has null year
            if (!entry.DateRelease.HasValue) return false;
            // year is integer AND entry has non-Null year
            int intYear = int.Parse(_releaseYearsFilterItems[index.Value]);
            return entry.DateRelease.Value.Year.Equals(intYear);
        };
        private Func<GameEntryViewModel, bool> completitionYearFilter(int? index) => entry =>
        {
            //no selectedIndex => no filtering
            if (index is null || index == -1) return true;
            //selectedIndex exists => item is "NoReleaseDate" or int
            if (_completitionYearsFilterItems[index.Value] == Assets.Resources.NoDateCompleted_Label)
                return !entry.DateCompleted.HasValue;
            // year is integer, check if entry has null year
            if (!entry.DateCompleted.HasValue) return false;
            // year is integer AND entry has non-Null year
            int intYear = int.Parse(_completitionYearsFilterItems[index.Value]);
            return entry.DateCompleted.Value.Year.Equals(intYear);
        };

        public ReactiveCommand<Unit, Unit> AnnulFilters { get; }
        private string _sortBy;
        public string SortBy
        {
            get => _sortBy;
            set => this.RaiseAndSetIfChanged(ref _sortBy, value);
        }
        private List<string> _sortOptions = [Assets.Resources.SortByOption_Title,
            Assets.Resources.SortByOption_Score,
            Assets.Resources.SortByOption_Status,
            Assets.Resources.SortByOption_Time,
            Assets.Resources.SortByOption_DateRelease,
            Assets.Resources.SortByOption_DateCompletition
            ];
        public List<string> SortOptions { get => _sortOptions; }

        public GamesPageViewModel(GameService service, IMessenger messenger)
        {
            _service = service;
            _messenger = messenger;
            _search = new SearchViewModel(service, messenger);
            _charts = new GameChartViewModel(service);
            LoadContents();
            _sortBy = SortOptions[0];

            OverlayClick = ReactiveCommand.Create(() => {
                SelectedEntry.ResetProperties();
                SelectedEntry = null!;
                OverlayOpacity = 0;
                
            });

            AnnulFilters = ReactiveCommand.Create(() =>
            {
                SelectedStatusFilterIndex = -1;
                SelectedScoreFilterIndex = -1;
                SelectedReleaseYearFilterIndex = -1;
                SelectedCompletitionYearFilterIndex = -1;
            });

            var statusPredicate = this.WhenAnyValue(x => x.SelectedStatusFilterIndex)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .DistinctUntilChanged()
                .Select(statusFilter)
            ;
            IObservable<Func<GameEntryViewModel, bool>>? scorePredicate = this.WhenAnyValue(x => x.SelectedScoreFilterIndex)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .DistinctUntilChanged()
                .Select(scoreFilter)
            ;
            IObservable<Func<GameEntryViewModel, bool>>? releaseYearPredicate = this.WhenAnyValue(x => x.SelectedReleaseYearFilterIndex)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .DistinctUntilChanged()
                .Select(releaseYearFilter)
            ;
            IObservable<Func<GameEntryViewModel, bool>>? completitionYearPredicate = this.WhenAnyValue(x => x.SelectedCompletitionYearFilterIndex)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .DistinctUntilChanged()
                .Select(completitionYearFilter)
            ;
            //sort
            var sortPredicate = this.WhenAnyValue(x => x.SortBy)
                .Select(x =>
                {
                    Debug.WriteLine(666);
                    if (x == Assets.Resources.SortByOption_Status) return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.Status);
                    if (x == Assets.Resources.SortByOption_Score) return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.Score);
                    if (x == Assets.Resources.SortByOption_Time) return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.Entry.Time);
                    if (x == Assets.Resources.SortByOption_DateCompletition) return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.DateCompleted);
                    if (x == Assets.Resources.SortByOption_DateRelease) return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.DateRelease);
                    return SortExpressionComparer<GameEntryViewModel>.Ascending(a => a.CustomTitle);
                });

            _sourceCache
                .Connect()
                .Filter(statusPredicate)
                .Filter(scorePredicate)
                .Filter(releaseYearPredicate)
                .Filter(completitionYearPredicate)
                .Sort(sortPredicate)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _contents)
                .Subscribe();
            _releaseYearsFilterItems.Clear();

            this
                .WhenAnyObservable(x => x.SelectedEntry.Changed)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => 
                {
                    if (x.PropertyName == "Entry" )
                    { 
                        _sourceCache.Refresh((GameEntryViewModel)x.Sender);
                        SelectedEntry = (GameEntryViewModel)x.Sender; }
                    
                });

            _messenger.Register<GamesPageViewModel, GameEntryDeleteMessage>(this, (_, message) =>
            {
                if (message is null) return;
                try
                {
                    service.Remove(message.Value.Entry.Id);

                    _sourceCache.Remove(message.Value);
                    Debug.WriteLine(message.Value.Entry.Id);
                    SelectedEntry = null!;
                    int? oldYear = message.Value.DateRelease.HasValue ? message.Value.DateRelease.Value.Year : null;
                    var c = 0;
                    if (oldYear is null)
                        c = TestViewModels.Where(x => x.DateRelease is null).Count();
                    else
                        c = TestViewModels.Where(x => x.DateRelease is not null && x.DateRelease.Value.Year == oldYear).Count();
                    if (c == 0)
                        _releaseYearsFilterItems.Remove(oldYear is null ? Assets.Resources.NoDateRelease_Label : oldYear.Value.ToString());
                }
                catch (ServiceUnavailableException e)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var key in e.Data.Keys) { 
                        sb.Append(e.Data[key]);
                    }

                    ErrorOverlay = new ErrorMessageOverlayViewModel(sb.ToString());
                }
            });
            _messenger.Register<GamesPageViewModel, Entry>(this, async (_, message) =>
            {
                if (message is GameEntry entry)
                {
                    var newEntryViewModel = new GameEntryViewModel(_service, entry);
                    _sourceCache.AddOrUpdate(newEntryViewModel);

                    string newYear = entry.Content.DateRelease.HasValue ? entry.Content.DateRelease.Value.Year.ToString() : Assets.Resources.NoDateRelease_Label;
                    _releaseYearsFilterItems.ReplaceOrAdd(newYear, newYear);

                    await newEntryViewModel.LoadCover();
                    Debug.WriteLine(message.Id);
                }

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
                _releaseYearsFilterItems
                    .AddRange(_sourceCache.Items.Select(x =>
                        x.DateRelease.HasValue ? x.DateRelease.Value.Year.ToString() : Assets.Resources.NoDateRelease_Label)
                    .Distinct()
                    .Order());
                _completitionYearsFilterItems
                    .AddRange(_sourceCache.Items.Select(x =>
                        x.DateCompleted.HasValue ? x.DateCompleted.Value.Year.ToString() : Assets.Resources.NoDateCompleted_Label)
                    .Distinct()
                    .Order());
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
            foreach (var content in _sourceCache.Items.ToList())
            {
                await content.LoadCover();
            }
        }

        public ReactiveCommand<Unit, Unit> OverlayClick { get; } 
    }
}
