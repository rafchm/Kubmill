using CommunityToolkit.Mvvm.Messaging;
using Kubmill.Configuration;
using Kubmill.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.Views
{
    public partial class Container : INavigationWindow, IRecipient<PageLoadingMessage>
    {
        public ViewModels.ContainerViewModel ViewModel { get; }

        public Container(ViewModels.ContainerViewModel viewModel, IPageService pageService, INavigationService navigationService,
            ISnackbarService snackbarService, IDialogService dialogService, IOptions<GeneralOptions> generalOptions)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            SetPageService(pageService);

            dialogService.SetDialogControl(RootDialog);
            RootDialog.ButtonRightClick += (s, e) => RootDialog.Hide();

            snackbarService.SetSnackbarControl(RootSnackbar);
            navigationService.SetNavigationControl(RootNavigation);

            StrongReferenceMessenger.Default.RegisterAll(this);

            Theme.Apply(generalOptions.Value.Theme);
        }

        #region INavigationWindow methods

        public Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow() 
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        public void Receive(PageLoadingMessage message)
        {
            var progress = message.Value;

            ViewModel.Progress = progress;
            ViewModel.IsIndeterminate = progress < 1;
            ViewModel.IsBusyVisibility = progress < 100 
                ? Visibility.Visible
                : Visibility.Hidden;
        }
    }
}