using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System.Reactive.Disposables;
using tc.ViewModels.Game;

namespace tc.Views.Game;

public partial class GameEntryView : ReactiveUserControl<GameEntryViewModel>
{
    public GameEntryView()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;
    }
}