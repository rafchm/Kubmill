using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kubmill.Messages;
using System;
using System.Threading;
using Wpf.Ui.Common.Interfaces;

namespace Kubmill.ViewModels
{
    public class PageViewModelBase : ObservableObject, INavigationAware
    {
        protected CancellationTokenSource? cts;

        private bool _isInitialized = false;
        private int _progress = -1;

        public bool IsBusy 
        {
            get { return _progress > 0 && _progress < 100; }
            set { SetProgress(value ? 0 : 100); }
        }

        protected void SetProgress(int percent)
        {
            _progress = Math.Min(100, percent);

            StrongReferenceMessenger.Default.Send(new PageLoadingMessage(_progress));

            if (_progress >= 100) _progress = -1;
        }

        public virtual void OnNavigatedFrom()
        {
            cts?.Cancel();
            cts = null;
            IsBusy = false;
        }

        public virtual void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                _isInitialized = OnLoadPage();
                cts ??= new CancellationTokenSource();
            }
        }

        public virtual bool OnLoadPage()
        {
            return true;
        }
    }
}
