using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using tc.ViewModels;

namespace tc.Views
{
    public partial class MainWindow : Window
    {
        // constructor with 1 parameter is needed to stop the DI to instantly create the window (when declared as singleton)
        // during the startup phase and crashing the whole android app
        // with "Specified method is not supported window" error
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            this.AttachDevTools();
            //this.WhenActivated(action =>action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }


        public MainWindow() : this(new MainViewModel())
        {
        }


        //private async Task DoShowDialogAsync(InteractionContext<SearchViewModel,
        //                                SearchItemViewModel?> interaction)
        //{
        //    var dialog = new SearchWindow();
        //    dialog.DataContext = interaction.Input;

        //    var result = await dialog.ShowDialog<SearchItemViewModel?>(this);
        //    interaction.SetOutput(result);
        //}
    }
}