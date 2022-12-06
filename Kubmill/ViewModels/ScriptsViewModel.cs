using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Configuration;
using Kubmill.Models.Scripting;
using Kubmill.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Common;

namespace Kubmill.ViewModels
{
    public partial class ScriptsViewModel : PageViewModelBase
    {
        private readonly IScriptRepository _scriptRepository;
        private readonly GeneralOptions _options;

        [ObservableProperty]
        private IEnumerable<ScriptFile>? _scripts;

        public ScriptsViewModel(IScriptRepository scriptRepository, IOptions<GeneralOptions> options)
            : base()
        {
            _scriptRepository = scriptRepository;
            _options = options.Value;
        }

        [RelayCommand]
        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await _scriptRepository.LoadScripts();
                Scripts = _scriptRepository.GetAllScripts().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void EditScript(ScriptFile script)
        {
            Process.Start(new ProcessStartInfo(_options.Editor, script.FileName)
            {
                UseShellExecute = true
            });
        }

        public override bool OnLoadPage()
        {
            Scripts = _scriptRepository.GetAllScripts();

            return true;
        }
    }
}
