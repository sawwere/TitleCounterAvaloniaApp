using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using tc.Models;
using tc.Service;

namespace tc.ViewModels.EntryViewModels
{
    public partial class FilmEntryViewModel: ViewModelBase
    {
        private static readonly string[] propertiesToMonitor = ["CustomTitle", "Status", "Score", "DateCompleted", "Note",];

        private readonly FilmEntry _entry;
        public FilmEntry Entry { get { return _entry; } }
        public FilmEntryViewModel(FilmEntry entry)
        {
            _entry = entry;
            _customTitle = _entry.CustomTitle;
            _rusTitle = _entry.Content.RusTitle;
            _score = _entry.Score;
            _status = _entry.Status;
            StatusComboBoxIndex = (new string[] { "completed", "backlog", "in progress", "retired" }).IndexOf(_status);
            _hours = _entry.Time / 60;
            _minutes = _entry.Time % 60;
            _note = _entry.Note;
            if (true)
            {
                _dateCompleted = new DateTimeOffset(_entry.DateCompleted, TimeOnly.MinValue, TimeSpan.Zero);
            }

            this.WhenAnyPropertyChanged(propertiesToMonitor).Subscribe(_ =>
            {
                Entry.CustomTitle = CustomTitle;
                Entry.Time = Hours * 60 + Minutes;
                Entry.Score = Score;
                Entry.Status = Status;
                Entry.DateCompleted = DateOnly.FromDateTime(DateCompleted.Date);
                Debug.WriteLine($"Update gameEntry {Entry.Id}");
                //GameService.Instance.Update(Entry);
            }
            );
        }

        [ObservableProperty]
        private string _customTitle;
        [ObservableProperty]
        private string? _rusTitle;

        [ObservableProperty]
        private string _status;
        [ObservableProperty]
        private int _statusComboBoxIndex;

        [ObservableProperty]
        private string? _note;
        public DateOnly DateRelease => _entry.Content.DateRelease.GetValueOrDefault();//TODO
        public Uri? LinkUrl => _entry.Content.LinkUrl is null ? null : new Uri(_entry.Content.LinkUrl);

        [ObservableProperty]
        private long _hours;

        [ObservableProperty]
        private long _minutes;

        [ObservableProperty]
        private long _score;

        [ObservableProperty]
        private DateTimeOffset _dateCompleted;

        [ObservableProperty]
        private Bitmap? _cover;

        public async Task LoadCover()
        {
            var data = await new HttpClient().GetByteArrayAsync($@"http://localhost:8080/images/films/{_entry.Content.Id}.jpg");
            await using (var imageStream = new MemoryStream(data))
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 64));
            }
            Debug.WriteLine("TODO: LOAD COVER FOR FILM_ENTRY");
        }


    }
}
