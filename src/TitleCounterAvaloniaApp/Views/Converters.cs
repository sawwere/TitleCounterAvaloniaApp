using Avalonia.Data.Converters;
using Avalonia.Media;
using tc.Models;

namespace tc.Views
{
    public static class Converters
    {
        /// <summary>
        /// Gets a Converter that takes a Entry score as input and converts it into a brush with specific color
        /// </summary>
        public static FuncValueConverter<long?, IBrush> MyConverter { get; } =
            new FuncValueConverter<long?, IBrush>(x =>
            {
                IBrush result = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
                if (x == null)
                    return result;
                switch (x)
                {
                    case 1:
                        result = new SolidColorBrush(Color.FromArgb(255, 255, 100, 0));
                        break;
                    case 2:
                        result = new SolidColorBrush(Color.FromArgb(255, 255, 160, 0));
                        break;
                    case 3:
                        result = new SolidColorBrush(Color.FromArgb(255, 255, 200, 0));
                        break;
                    case 4:
                        result = new SolidColorBrush(Color.FromArgb(255, 255, 240, 0));
                        break;
                    case 5:
                        result = new SolidColorBrush(Color.FromArgb(255, 200, 240, 110));
                        break;
                    case 6:
                        result = new SolidColorBrush(Color.FromArgb(255, 180, 255, 0));
                        break;
                    case 7:
                        result = new SolidColorBrush(Color.FromArgb(255, 70, 200, 0));
                        break;
                    case 8:
                        result = new SolidColorBrush(Color.FromArgb(255, 60, 170, 0));
                        break;
                    case 9:
                        result = new SolidColorBrush(Color.FromArgb(255, 50, 140, 0));
                        break;
                    case 10:
                        result = new SolidColorBrush(Color.FromArgb(255, 40, 110, 36));
                        break;
                    default: break;

                }
                return result;
            });

        public static FuncValueConverter<Entry, bool> IsEntryCompleted { get; } =
            new FuncValueConverter<Entry, bool>(x => x.Status.Equals("completed")
            );
    }
}
