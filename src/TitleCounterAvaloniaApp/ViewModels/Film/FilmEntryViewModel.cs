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

namespace tc.ViewModels.Film
{
    public partial class FilmEntryViewModel : ViewModelBase
    {
        private static readonly string[] propertiesToMonitor = ["CustomTitle", "Status", "Score", "DateCompleted", "Note",];

        private readonly FilmEntry _entry;
        public FilmEntry Entry { get { return _entry; } }
        public FilmEntryViewModel(FilmEntry entry)
        {
            _entry = entry;
            _customTitle = _entry.CustomTitle;
            _alternativeTitle = _entry.Content.AlternativeTitle;
            _score = _entry.Score;
            _status = _entry.Status;
            StatusComboBoxIndex = (new string[] { "completed", "backlog", "in progress", "retired" }).IndexOf(_status);
            if (_entry.Content.Time is not null)
            {
                _hours = _entry.Content.Time / 60;
                _minutes = _entry.Content.Time % 60;
            }

            _note = _entry.Note;
            if (_entry.DateCompleted is not null)
            {
                _dateCompleted = new DateTimeOffset(_entry.DateCompleted.Value, TimeOnly.MinValue, TimeSpan.Zero);
            }

            this.WhenAnyPropertyChanged(propertiesToMonitor).Subscribe(_ =>
            {
                Entry.CustomTitle = CustomTitle;
                Entry.Score = Score;
                Entry.Status = Status;
                if (DateCompleted is not null)
                {
                    Entry.DateCompleted = DateOnly.FromDateTime(DateCompleted.Value.Date);
                }
                Debug.WriteLine($"Update gameEntry {Entry.Id}");
                //GameService.Instance.Update(Entry);
            }
            );
        }

        //[ObservableProperty]
        private string _customTitle;
        public string CustomTitle
        {
            get { return _customTitle; }
            set { this.RaiseAndSetIfChanged(ref _customTitle, value); }
        }

        //[ObservableProperty]
        private string? _alternativeTitle;
        public string AlternativeTitle
        {
            get { return _alternativeTitle; }
            set { this.RaiseAndSetIfChanged(ref _alternativeTitle, value); }
        }

        // [ObservableProperty]
        private string _status;
        public string Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        //[ObservableProperty]
        private int _statusComboBoxIndex;
        public int StatusComboBoxIndex
        {
            get { return _statusComboBoxIndex; }
            set { this.RaiseAndSetIfChanged(ref _statusComboBoxIndex, value); }
        }

        //[ObservableProperty]
        private string? _note;
        public string? Note
        {
            get { return _note; }
            set { this.RaiseAndSetIfChanged(ref _note, value); }
        }
        public DateOnly? DateRelease => _entry.Content.DateRelease;
        public Uri? LinkUrl => _entry.Content.LinkUrl is null ? null : new Uri(_entry.Content.LinkUrl);

        //[ObservableProperty]
        private long? _hours;
        public long? Hours
        {
            get { return _hours; }
            set { this.RaiseAndSetIfChanged(ref _hours, value); }
        }

        //[ObservableProperty]
        private long? _minutes;
        public long? Minutes
        {
            get { return _minutes; }
            set { this.RaiseAndSetIfChanged(ref _minutes, value); }
        }

        //[ObservableProperty]
        private long? _score;
        public long? Score
        {
            get { return _score; }
            set { this.RaiseAndSetIfChanged(ref _score, value); }
        }

        //[ObservableProperty]
        private DateTimeOffset? _dateCompleted;
        public DateTimeOffset? DateCompleted
        {
            get { return _dateCompleted; }
            set { this.RaiseAndSetIfChanged(ref _dateCompleted, value); }
        }

        //[ObservableProperty]
        private Bitmap? _cover;
        public Bitmap? Cover
        {
            get { return _cover; }
            set { this.RaiseAndSetIfChanged(ref _cover, value); }
        }

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
