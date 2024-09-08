using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Linq;
using tc.ViewModels;
using tc.ViewModels.Game;

namespace tc.Views.Game;

public partial class GameChartsView : ReactiveUserControl<GameChartViewModel>
{
    public GameChartsView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            
        });
    }
}