using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using tc.ViewModels;

namespace tc.Views;

public partial class SearchWindow : ReactiveWindow<SearchViewModel>
{
    public SearchWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;

        this.WhenActivated(action => action(ViewModel!.AddContent.Subscribe(Close)));
    }
}