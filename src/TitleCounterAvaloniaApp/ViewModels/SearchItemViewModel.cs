using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Diagnostics;
using System.Threading.Tasks;
using tc.Dto;

namespace tc.ViewModels
{
    public partial class SearchItemViewModel : ViewModelBase
    {
        private readonly ISearchable _item;
        public ISearchable Item { get { return _item; } }

        public SearchItemViewModel(ISearchable item)
        {
            _item = item;
        }
        public long Id => _item.Id;
        public string Title => _item.Title;
        public string DateRelease => _item.DateRelease;

        private Bitmap? _cover;

        public Bitmap? Cover
        {
            get => _cover;
            private set => this.RaiseAndSetIfChanged(ref _cover, value);
        }

        public async Task LoadCover()
        {
            //await using (var imageStream = await GameService.Instance.LoadCoverBitmapAsync(_item.Id))
            //{
            //    Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            //}
            Debug.WriteLine($"TODO: Load cover for {_item.Id}");
        }
    }
}
