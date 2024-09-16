using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using tc.Models;
using tc.Service;
using tc.Utils.Exception;
using tc.Utils.Messages;
using ValidationContext = ReactiveUI.Validation.Contexts.ValidationContext;

namespace tc.ViewModels.Game
{
    public partial class GameEntryViewModel : ViewModelBase
    {
        private readonly GameService _service;
        private static readonly string[] propertiesToMonitor = ["CustomTitle", "Status", "Score", "DateCompleted", "Note", "Hours", "Minutes"];
        private static readonly string[] statuses = ["completed", "backlog", "in progress", "retired"];
        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenHltbLinkCommand { get; }

        private readonly GameEntry _entry;
        public GameEntry Entry { get { return _entry; } }
        public GameEntryViewModel(GameService service, GameEntry entry)
        {
            //ValidationContext = new ValidationContext();
            _service = service;
            _entry = entry;
            ResetProperties();

            if (_entry.DateCompleted is not null)
            {
                _dateCompleted = new DateTimeOffset(_entry.DateCompleted.Value, TimeOnly.MinValue, TimeSpan.Zero);
            }

            this.WhenPropertyChanged(x => x.StatusComboBoxIndex)
                .Subscribe(x => OnStatusComboBoxIndexChanged(x.Value));

            this.ValidationRule(viewModel => viewModel.CustomTitle, 
                name => !string.IsNullOrWhiteSpace(name) && name.Length <= 64,
                Assets.Resources.EntryAttribute_CustomTitle_ValidationMessage);
            this.ValidationRule(viewModel => viewModel.Minutes,
                item => {
                    if (int.TryParse(item, out int parseResult))
                    {
                        if (parseResult >= 0 && parseResult < 60)
                            return true;
                        return false;
                    }
                    return false;
                },
                Assets.Resources.EntryAttribute_Minutes_ValidationMessage);
            this.ValidationRule(viewModel => viewModel.Hours,
                item => {
                    if (int.TryParse(item, out int parseResult))
                    {
                        if (parseResult >= 0 && parseResult < 60)
                            return true;
                        return false;
                    }
                    return false;
                },
                Assets.Resources.EntryAttribute_Hours_ValidationMessage);
            this.ValidationRule(viewModel => viewModel.Note,
                note => note is null? true : note.Length <= 512,
                Assets.Resources.EntryAttribute_Note_ValidationMessage);
            this.OpenHltbLinkCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = HltbLink,
                    UseShellExecute = true
                });
            });


            SaveChangesCommand = ReactiveCommand.CreateFromTask(async () => 
            {
                try
                {
                    if (ValidationContext.IsValid)
                    {
                        GameEntry clone = (_entry.Clone() as GameEntry)!;
                        clone.CustomTitle = CustomTitle;
                        if (HasCustomTime)
                        {
                            clone.Time = int.Parse(Hours!) * 60 + int.Parse(_minutes!);
                        }
                        clone.Score = Score;
                        clone.Status = Status;
                        if (DateCompleted is not null)
                        {
                            clone.DateCompleted = DateOnly.FromDateTime(DateCompleted.Value.Date);
                        }
                        clone.Note = Note;
                        await _service.Update(clone);
                        _entry.CustomTitle = CustomTitle;
                        if (HasCustomTime)
                        {
                            _entry.Time = int.Parse(Hours!) * 60 + int.Parse(_minutes!);
                        }
                        _entry.Score = Score;
                        _entry.Status = Status;
                        if (DateCompleted is not null)
                        {
                            _entry.DateCompleted = DateOnly.FromDateTime(DateCompleted.Value.Date);
                        }
                        _entry.Note = Note;
                        Debug.WriteLine($"Update gameEntry {Entry.Id}");
                    }
                    else
                    {
                        Debug.WriteLine(ValidationContext.Text);
                    }
                }
                catch (ServiceUnavailableException ex)
                {

                }
            });
        }

        private bool hasCustomTime = false;
        public bool HasCustomTime
        {
            get { return hasCustomTime; }
            set => this.RaiseAndSetIfChanged(ref hasCustomTime, value);
        }

        private string _customTitle;
        public string CustomTitle
        {
            get { return _customTitle; }
            set { this.RaiseAndSetIfChanged(ref _customTitle, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        private string? _note;
        public string? Note
        {
            get { return _note; }
            set { 
                if (value == "")
                    value = null;
                this.RaiseAndSetIfChanged(ref _note, value); 
            }
        }
        public DateOnly? DateRelease => _entry.Content.DateRelease.HasValue ? _entry.Content.DateRelease.Value : null;//TODO
        public string? HltbLink => _entry.Content.LinkUrl is null ? null : "https://howlongtobeat.com/game/" +Entry.Content.ExternalId.HltbId;
        // String type because of validation of associated textBox
        private string? _hours;
        public string? Hours
        {
            get { return _hours; }
            set {
                this.RaiseAndSetIfChanged(ref _hours, value);
            }
        }
        // String type because of validation of associated textBox
        private string? _minutes;
        public string? Minutes
        {
            get { return _minutes?.ToString(); }
            set
            {
                this.RaiseAndSetIfChanged(ref _minutes, value);
            }
        }

        private long? _score;
        public long? Score
        {
            get { return _score; }
            set { this.RaiseAndSetIfChanged(ref _score, value); }
        }

        private DateTimeOffset? _dateCompleted;
        public DateTimeOffset? DateCompleted
        {
            get { return _dateCompleted; }
            set { this.RaiseAndSetIfChanged(ref _dateCompleted, value); }
        }

        private int _statusComboBoxIndex;
        public int StatusComboBoxIndex
        {
            get { return _statusComboBoxIndex; }
            set { this.RaiseAndSetIfChanged(ref _statusComboBoxIndex, value); }
        }

        private void OnStatusComboBoxIndexChanged(int newValue)
        {
            switch (newValue)
            {
                case 0:
                    Status = "completed";
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
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        private void Delete()
        {
            Debug.WriteLine($"Deleting {_entry.Id}");
            Ioc.Default.GetRequiredService<IMessenger>().Send(new GameEntryDeleteMessage(this));
        }
        

        private Bitmap? _cover;
        public Bitmap? Cover
        {
            get { return _cover; }
            set { this.RaiseAndSetIfChanged(ref _cover, value); }
        }

        public void ResetProperties()
        {
            CustomTitle = _entry.CustomTitle;
            Score = _entry.Score;
            Status = _entry.Status;
            StatusComboBoxIndex = statuses.IndexOf(_status);

            if (_entry.Time is not null)
            {
                hasCustomTime = true;
                Hours = (_entry.Time / 60).ToString();
                Minutes = (_entry.Time % 60).ToString();
            }
            else if (_entry.Content.Time is not null)
            {
                Hours = (_entry.Content.Time / 60).ToString();
                Minutes = (_entry.Content.Time % 60).ToString();
            }

            Note = _entry.Note;

            if (_entry.DateCompleted is not null)
            {
                DateCompleted = new DateTimeOffset(_entry.DateCompleted.Value, TimeOnly.MinValue, TimeSpan.Zero);
            }
        }

        public async Task LoadCover()
        {
            var data = await _service.LoadCoverAsync(Entry.Content.Id);
            using var imageStream = new MemoryStream(data);
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 64));
        }
    }
}
