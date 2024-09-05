using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using tc.Models;
using tc.Service;
using tc.Utils.Messages;

namespace tc.ViewModels.EntryViewModels
{
    public partial class GameEntryViewModel : ViewModelBase
    {
        private readonly GameService _service;
        private static readonly string[] propertiesToMonitor = ["CustomTitle", "Status", "Score", "DateCompleted", "Note", "Hours", "Minutes"];
        private static readonly string[] statuses = ["completed", "backlog", "in progress", "retired"];
        private readonly GameEntry _entry;
        public GameEntry Entry { get { return _entry; } }
        public GameEntryViewModel(GameService service, GameEntry entry)
        {
            _service = service;
            _entry = entry;
            CustomTitle = _entry.CustomTitle;
            _score = _entry.Score;
            _status = _entry.Status;
            StatusComboBoxIndex = statuses.IndexOf(_status);

            if (_entry.Time is not null)
            {
                hasCustomTime = true;
                _hours = _entry.Time / 60;
                _minutes = _entry.Time % 60;
            } 
            else if (_entry.Content.Time is not null)
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
                if (!HasErrors)
                {
                    Entry.CustomTitle = CustomTitle;
                    if (HasCustomTime)
                    {
                        Entry.Time = Hours * 60 + Minutes;
                    }
                    Entry.Score = Score;
                    Entry.Status = Status;
                    if (DateCompleted is not null)
                    {
                        Entry.DateCompleted = DateOnly.FromDateTime(DateCompleted.Value.Date);
                    }
                    Debug.WriteLine($"Update gameEntry {Entry.Id}");
                    //_service.Update(entry);
                }
                Debug.WriteLine($"Update gameEntry {S}");
            }
            );
        }

        [ObservableProperty]
        private bool hasCustomTime = false;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MinLength(1)]
        private string _customTitle;

        //[EmailAddress]
        //public string CustomTitle
        //{
        //    get => _customTitle;
        //    set
        //    {
        //        if (value.Length > 0 && value.Length < 10)
        //            this.SetProperty(ref _customTitle, value);
        //        else
        //            AddError("CustomProperty", "len");
        //        Debug.WriteLine(_customTitle);
        //    }
        //}
        public string S
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        [ObservableProperty]
        private string _status;

        [ObservableProperty]
        private string? _note;
        public DateOnly? DateRelease => _entry.Content.DateRelease.GetValueOrDefault();//TODO
        public Uri? LinkUrl => _entry.Content.LinkUrl is null ? null : new Uri(_entry.Content.LinkUrl);

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, 100000)]
        private long? _hours;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, 59)]
        private long? _minutes;

        partial void OnHoursChanged(long? value)
        {
            HasCustomTime = true;
        }

        partial void OnMinutesChanged(long? value)
        {
            HasCustomTime = true;
        }

        [ObservableProperty]
        private long? _score;

        [ObservableProperty]
        private DateTimeOffset? _dateCompleted;

        [ObservableProperty]
        private int _statusComboBoxIndex;

        partial void OnStatusComboBoxIndexChanged(int oldValue, int newValue)
        {
            switch (newValue)
            {
                case 0:
                    Status = "completed";
                    DateCompleted = DateTimeOffset.Now;
                    break;
                case 1:
                    Status = "backlog";
                    break;
                case 2:
                    Status = "in progress";
                    break;
                case 3:
                    Status = "retired";
                    break;
                default:
                    break;
            }
            S = Status;
        }

        [RelayCommand]
        private void Delete()
        {
            Debug.WriteLine($"Deleting {_entry.Id}");
            Ioc.Default.GetRequiredService<IMessenger>().Send(new GameEntryDeleteMessage(this));
        }

        [ObservableProperty]
        private Bitmap? _cover;

        

        public async Task LoadCover()
        {
            var data = await _service.LoadCoverAsync(Entry.Content.Id);
            using var imageStream = new MemoryStream(data);
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 64));
            //Debug.WriteLine("TODO: LOAD COVER FOR GAME_ENTRY");
        }
    }
}
