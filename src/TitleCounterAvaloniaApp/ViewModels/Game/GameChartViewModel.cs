using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tc.Models;
using tc.Service;
using System.Diagnostics;
using System.Threading;
using tc.Utils.Exception;
using System.Collections.Immutable;
using LiveChartsCore.Kernel;
using LiveChartsCore.Defaults;

namespace tc.ViewModels.Game
{
    public class GameChartViewModel: ViewModelBase
    {
        private readonly GameService _service;
        public List<GameEntry> Contents { get; set; } = new List<GameEntry>();

        public ISeries[] StatusSeries { get => Contents
                .GroupBy(x => x.Status)
                .Select(x => new PieSeries<int>() { Values =  [x.Count()], Name = x.Key}).ToArray(); }

        public ISeries[] CompletedByYear { get {
                Dictionary<int, int> dict = new Dictionary<int, int>();
                int completed = 0;
                foreach (var item in Contents.Where(x => x.DateCompleted is not null).OrderBy(x=>x.DateCompleted).GroupBy(x => x.DateCompleted!.Value.Year))
                {
                    var today = (item.Key, item.Count(x=>x.Status=="completed"));
                    completed = completed + today.Item2;
                    dict.Add(today.Key, completed);
                }

                return [new StackedAreaSeries<KeyValuePair<int, int>>() { Values = dict, Mapping= (sample, _) => new Coordinate( sample.Key, sample.Value)}];
            }
        }

        public GameChartViewModel(GameService service)
        {
            _service = service;
            LoadContents();
        }

        private async void LoadContents()
        {
            try
            {
                Contents = (await _service.FindAll()).Select(x => (x as GameEntry)!).ToList();
            }
            catch (ServiceUnavailableException ex)
            {
                //ErrorOverlay = new ErrorMessageOverlayViewModel(Assets.Resources.ServiceUnavableMessage);
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
