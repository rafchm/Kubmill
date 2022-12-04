using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Models.Scripting;
using Kubmill.Repositories;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kubmill.ViewModels
{
    public partial class ScriptsViewModel : PageViewModelBase
    {
        private readonly IScriptRepository _scriptRepository;

        [ObservableProperty]
        private IEnumerable<ScriptFile>? _scripts;

        public ScriptsViewModel(IScriptRepository scriptRepository) 
            : base()
        {
            _scriptRepository = scriptRepository;
        }

        [RelayCommand]
        public static void EditScript(ScriptFile script)
        {
            Process.Start(new ProcessStartInfo("notepad", script.FileName)
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
