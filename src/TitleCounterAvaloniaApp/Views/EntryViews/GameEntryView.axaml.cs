using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using tc.ViewModels.EntryViewModels;

namespace tc.Views.EntryViews;

public partial class GameEntryView : ReactiveUserControl<GameEntryViewModel>
{
    public GameEntryView()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;
    }

    public void TextBox_TextChanging(object sender, TextChangedEventArgs e)
    {

    }
}