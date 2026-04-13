using System.Windows;
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
        }
    }
}
