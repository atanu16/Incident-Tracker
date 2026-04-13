using System.Windows.Input;
using IncidentTracker.Commands;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IExcelService _excelService;
        private int _selectedTabIndex;
        private bool _isDarkMode = true;
        private bool _isLoading;

        public DashboardViewModel DashboardVM { get; }
        public AllRecordsViewModel AllRecordsVM { get; }
        public EditViewModel EditVM { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (SetField(ref _selectedTabIndex, value))
                    _ = LoadCurrentTabAsync();
            }
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                SetField(ref _isDarkMode, value);
                ThemeService.ApplyTheme(value);
                OnPropertyChanged(nameof(ThemeIcon));
            }
        }

        public string ThemeIcon => IsDarkMode ? "☀" : "🌙";

        public ICommand GlobalRefreshCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public MainViewModel(IExcelService excelService)
        {
            _excelService = excelService;

            void SetLoading(bool v) => IsLoading = v;

            DashboardVM = new DashboardViewModel(excelService, SetLoading);
            AllRecordsVM = new AllRecordsViewModel(excelService, SetLoading);
            EditVM = new EditViewModel(excelService, SetLoading);

            GlobalRefreshCommand = new AsyncRelayCommand(LoadCurrentTabAsync);
            ToggleThemeCommand = new RelayCommand(() => IsDarkMode = !IsDarkMode);
        }

        public async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                await DashboardVM.LoadDataAsync();
                NotificationService.Instance.Show("Welcome", "Developed by Atanu Bera ❤️", NotificationType.Info, 3000);
            }
            finally { IsLoading = false; }
        }

        public async Task RefreshAllAsync()
        {
            await DashboardVM.LoadDataAsync();
            if (_selectedTabIndex == 1) await AllRecordsVM.LoadDataAsync();
            if (_selectedTabIndex == 2) await EditVM.LoadDataAsync();
        }

        private async Task LoadCurrentTabAsync()
        {
            switch (_selectedTabIndex)
            {
                case 0: await DashboardVM.LoadDataAsync(); break;
                case 1: await AllRecordsVM.LoadDataAsync(); break;
                case 2: await EditVM.LoadDataAsync(); break;
            }
        }
    }
}
