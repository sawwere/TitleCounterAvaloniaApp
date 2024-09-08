using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tc.Models;

namespace tc.Utils.Converters
{
    public static class FuncConverters
    {
        public static FuncValueConverter<string[], string> ArrayToString { get; } =
            new FuncValueConverter<string[], string> (x =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var str in x)
                {
                    sb.Append(str);
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            });
    }
}
