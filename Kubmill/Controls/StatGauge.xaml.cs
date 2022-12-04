using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Wpf.Ui.Controls;

namespace Kubmill.Controls
{
    public partial class StatGauge : UserControl
    {
        public int Current
        {
            get { return (int)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        public static readonly DependencyProperty
            CurrentProperty = DependencyProperty.Register(nameof(Current), typeof(int),
                typeof(UserControl), new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DataChanged));

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        public static readonly DependencyProperty
            TotalProperty = DependencyProperty.Register(nameof(Total), typeof(int), 
                typeof(UserControl), new FrameworkPropertyMetadata(0, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DataChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty
            TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
                typeof(UserControl), new FrameworkPropertyMetadata("", 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TitleChanged));

        private void Update()
        {
            statLabel.Text = $"{Current}/{Total}";
            
            var progress = Total > 0 ? 100 * Current / Total : 0;

            if (progress == 0)
            {
                gaugeDial.Progress = 0;
                return;
            }

            gaugeDial.BeginAnimation(ProgressRing.ProgressProperty, 
                new DoubleAnimation 
                {
                    From = 0,
                    To = progress,
                    Duration = new Duration(TimeSpan.FromSeconds(1.2)),
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
                });
        }

        private static void DataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((StatGauge)sender).Update();
        }

        private static void TitleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((StatGauge)sender).titleLabel.Text = e.NewValue.ToString();
        }

        public StatGauge()
        {
            InitializeComponent();
        }
    }
}
