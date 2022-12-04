using Wpf.Ui.Common.Interfaces;

namespace Kubmill.Views.Pages
{
    /// <summary>
    /// Interaction logic for ScriptsPage.xaml
    /// </summary>
    public partial class ScriptsPage : INavigableView<ViewModels.ScriptsViewModel>
    {
        public ViewModels.ScriptsViewModel ViewModel
        {
            get;
        }

        public ScriptsPage(ViewModels.ScriptsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
