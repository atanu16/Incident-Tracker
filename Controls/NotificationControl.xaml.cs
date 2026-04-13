using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.Controls
{
    public partial class NotificationControl : UserControl
    {
        public NotificationControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not NotificationItem item) return;

            await Task.Delay(item.DurationMs);

            if (!IsLoaded) return;

            // Animate out
            var sb = new Storyboard();

            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTarget(fadeOut, RootBorder);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(UIElement.OpacityProperty));

            var slideOut = new DoubleAnimation(0, 60, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTarget(slideOut, SlideTransform);
            Storyboard.SetTargetProperty(slideOut, new PropertyPath(TranslateTransform.XProperty));
            slideOut.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn };

            sb.Children.Add(fadeOut);
            sb.Children.Add(slideOut);
            sb.Completed += (_, _) => NotificationService.Instance.Remove(item);
            sb.Begin();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is NotificationItem item)
                NotificationService.Instance.Remove(item);
        }
    }
}
