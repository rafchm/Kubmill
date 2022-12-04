using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace Kubmill.ViewModels
{
    public partial class OutputDataViewModel : ObservableObject
    {
        private static int _counter = 1;

        public string? Data { get; set; }

        [RelayCommand]
        public void Copy()
        {
            if (!string.IsNullOrEmpty(Data))
            {
                Clipboard.SetText(Data);
            }
        }

        [RelayCommand]
        public void Open()
        {
            if (!string.IsNullOrEmpty(Data))
            {
                var cnt = Interlocked.Increment(ref _counter);
                var fileName = $"script_out-{DateTime.Now:yyyy-mm-dd_HH.mm.ss.}{cnt:000}.txt";
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllText(filePath, Data);
                Process.Start(new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                });
            }
        }
    }
}
