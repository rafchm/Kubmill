using Kubmill.Models.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Kubmill.Views.Windows
{
    public partial class ScriptOutputWindow : Wpf.Ui.Controls.UiWindow
    {
        public ViewModels.OutputDataViewModel ViewModel { get; }

        public ScriptOutputWindow(ViewModels.OutputDataViewModel viewModel)
        {
            DataContext = this;
            ViewModel = viewModel;

            InitializeComponent();
        }

        public void AddResult(ScriptResult result)
        {
            Dispatcher.InvokeAsync(() =>
            {
                progressBar.IsIndeterminate = true;
                progressBar.Visibility = Visibility.Collapsed;
            });

            if (result != null)
            {
                AddLines(result.Errors);

                if (result.Output != null)
                {
                    ViewModel.Data = string.Join(Environment.NewLine, result.Output);

                    AddLines(result.Output, ScriptEventType.Information);
                }
            }
        }

        public void RecordEvent(ScriptEvent sev)
        {
            Dispatcher.InvokeAsync(() =>
            {
                AddLine(sev.Message, sev.EventType);

                progressBar.IsIndeterminate = sev.Progress == 0;

                if (!progressBar.IsIndeterminate)
                {
                    progressBar.Value = Math.Min(100, Math.Max(0, sev.Progress));
                }
            });
        }

        public void AddLine(string msg, ScriptEventType eventType)
        {
            scriptOutputText.Inlines.Add(
                new Run(msg + Environment.NewLine)
                {
                    Foreground = GetBrush(eventType)
                });

            scrollViewer.ScrollToEnd();
        }

        public void AddLines(IEnumerable<ErrorRecord>? errors)
        {
            var lines = errors?.Select(e => e.ErrorDetails?.Message ?? e.Exception?.Message ?? "Unknown error!");
            AddLines(lines, ScriptEventType.Error);
        }

        public void AddLines(IEnumerable<string>? lines, ScriptEventType eventType)
        {
            if (lines == null || !lines.Any()) return;

            var runs = lines.Select(l => new Run(l + Environment.NewLine)
            {
                Foreground = GetBrush(eventType)
            });

            Dispatcher.InvokeAsync(() =>
            {
                scriptOutputText.Inlines.AddRange(runs);
                scrollViewer.ScrollToEnd();
            });
        }

        private static SolidColorBrush GetBrush(ScriptEventType eventType)
        {
            return eventType switch
            {
                ScriptEventType.Debug => Brushes.LightGray,
                ScriptEventType.Warning => Brushes.LightGoldenrodYellow,
                ScriptEventType.Error => Brushes.Red,
                ScriptEventType.Progress => Brushes.LightBlue,
                _ => Brushes.WhiteSmoke,
            };
        }
    }
}
