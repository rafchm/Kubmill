using Wpf.Ui.Common.Interfaces;

namespace Kubmill.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : INavigableView<ViewModels.HomeViewModel>
    {
        public ViewModels.HomeViewModel ViewModel
        {
            get;
        }

        public HomePage(ViewModels.HomeViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
