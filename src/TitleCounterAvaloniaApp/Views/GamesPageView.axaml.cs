using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using tc.ViewModels;

namespace tc.Views;

public partial class GamesPageView : ReactiveUserControl<GamesPageViewModel>
{
    public GamesPageView()
    {
        InitializeComponent();
        this.WhenActivated(x => 
        {
            //this.BindCommand(this.ViewModel!,
            //                 vm => vm.OverlayClick,
            //                 v => v.entryOverlay);
        });
        this.entryOverlay.PointerPressed += EntryOverlay_PointerPressed;
    }

    private void EntryOverlay_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        this.ViewModel!.OverlayClick.Execute().Subscribe();
    }
}