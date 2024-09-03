using DynamicData;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using tc.Models;
using tc.Service;
using tc.ViewModels.EntryViewModels;

namespace tc.ViewModels
{
    public partial class FilmsPageViewModel : ViewModelBase
    {
        private readonly FilmService _service;
        public ObservableCollection<FilmEntryViewModel> Contents { get; } = [];
        private FilmEntryViewModel _selectedEntry;
        public FilmEntryViewModel SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedEntry, value);
            }
        }
        public ICommand SearchCommand { get; }
        public Interaction<SearchViewModel, SearchItemViewModel?> ShowDialog { get; }

        public FilmsPageViewModel(FilmService service)
        {
            _service = service;
            LoadContents();
        }

        private async void LoadContents()
        {
            IEnumerable<FilmEntryViewModel> contents;
            contents = (await _service.Load()).Select(x => new FilmEntryViewModel(x as FilmEntry));

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
