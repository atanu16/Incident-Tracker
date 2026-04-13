using System.Windows.Controls;
using IncidentTracker.Services;
using IncidentTracker.ViewModels;

namespace IncidentTracker.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DashboardViewModel vm)
            {
                vm.OpenAddNewRequested -= OnOpenAddNewRequested;
                vm.OpenAddNewRequested += OnOpenAddNewRequested;
            }
        }

        private void OnOpenAddNewRequested()
        {
            var mainWindow = System.Windows.Window.GetWindow(this);
            if (mainWindow?.DataContext is not MainViewModel mainVm) return;

            var addVm = new AddRecordViewModel(App.ExcelService);
            var win = new AddRecordWindow
            {
                DataContext = addVm,
                Owner = mainWindow
            };

            addVm.SubmitSucceeded += async () =>
            {
                win.Close();
                await mainVm.RefreshAllAsync();
            };

            addVm.CancelRequested += () => win.Close();

            win.ShowDialog();
        }
    }
}
