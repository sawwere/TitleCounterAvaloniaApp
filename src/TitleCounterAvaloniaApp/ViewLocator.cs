using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Diagnostics;
using tc.ViewModels;

namespace tc
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                return null;
            Debug.WriteLine(data.GetType().FullName!);
            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}