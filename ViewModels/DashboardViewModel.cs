using System.Collections.ObjectModel;
using System.Windows.Input;
using IncidentTracker.Commands;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IExcelService _excelService;
        private readonly Action<bool> _setLoading;
        private string _searchText = "";
        private bool _isLoading;
        private ObservableCollection<IncidentRecord> _displayedRecords = new();
        private List<IncidentRecord> _allInProgress = new();
        private int _totalInProgress;

        public ObservableCollection<IncidentRecord> DisplayedRecords
        {
            get => _displayedRecords;
            private set => SetField(ref _displayedRecords, value);
        }

        public string SearchText
        {
            get => _searchText;
            set { SetField(ref _searchText, value); ApplyFilter(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { SetField(ref _isLoading, value); _setLoading(value); }
        }

        public int TotalInProgress
        {
            get => _totalInProgress;
            private set => SetField(ref _totalInProgress, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddNewCommand { get; }

        public event Action? OpenAddNewRequested;

        public DashboardViewModel(IExcelService excelService, Action<bool> setLoading)
        {
            _excelService = excelService;
            _setLoading = setLoading;
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);
            AddNewCommand = new RelayCommand(() => OpenAddNewRequested?.Invoke());
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var records = await _excelService.LoadRecordsAsync();
                _allInProgress = records.Where(r => r.Status == "InProgress").ToList();
                TotalInProgress = _allInProgress.Count;
                ApplyFilter();
            }
            catch (Exception ex)
            {
                NotificationService.Instance.Show("Error", $"Failed to load data: {ex.Message}", NotificationType.Error);
            }
            finally { IsLoading = false; }
        }

        private void ApplyFilter()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allInProgress
                : _allInProgress.Where(r =>
                    r.SubjectLine.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Incident.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.ShortDescription.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.CreatedBy.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            DisplayedRecords = new ObservableCollection<IncidentRecord>(filtered);
        }
    }
}
