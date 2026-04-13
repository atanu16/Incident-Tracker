using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using IncidentTracker.ViewModels;

namespace IncidentTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel(App.ExcelService);
            DataContext = vm;
            Loaded += async (_, _) => await vm.InitializeAsync();
            SizeChanged += OnSizeChanged;
            StateChanged += OnStateChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateClipRect();
        }

        private void OnStateChanged(object? sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Remove rounded corners and margin when maximized
                WindowBorder.CornerRadius = new CornerRadius(0);
                WindowBorder.Margin = new Thickness(0);
                WindowBorder.BorderThickness = new Thickness(0);
                WindowBorder.Clip = null;
            }
            else
            {
                WindowBorder.CornerRadius = new CornerRadius(8);
                WindowBorder.Margin = new Thickness(6);
                WindowBorder.BorderThickness = new Thickness(1);
                UpdateClipRect();
            }
        }

        private void UpdateClipRect()
        {
            if (WindowState == WindowState.Maximized) return;
            var w = WindowBorder.ActualWidth;
            var h = WindowBorder.ActualHeight;
            if (w > 0 && h > 0)
                ClipRect.Rect = new System.Windows.Rect(0, 0, w, h);
        }

        // Custom title bar drag
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    var pos = e.GetPosition(this);
                    var screenPos = PointToScreen(pos);
                    WindowState = WindowState.Normal;
                    Left = screenPos.X - (Width / 2);
                    Top = screenPos.Y - 20;
                }
                DragMove();
            }
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
            => ToggleMaximize();

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
            => Close();

        private void ToggleMaximize()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }
}
