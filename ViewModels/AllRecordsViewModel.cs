using System.Collections.ObjectModel;
using System.Windows.Input;
using IncidentTracker.Commands;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.ViewModels
{
    public class AllRecordsViewModel : BaseViewModel
    {
        private readonly IExcelService _excelService;
        private readonly Action<bool> _setLoading;
        private string _searchText = "";
        private bool _isLoading;
        private bool _sortAscending = false;
        private ObservableCollection<IncidentRecord> _displayedRecords = new();
        private List<IncidentRecord> _allRecords = new();
        private int _totalCount;

        public ObservableCollection<IncidentRecord> DisplayedRecords
        {
            get => _displayedRecords;
            private set => SetField(ref _displayedRecords, value);
        }

        public string SearchText
        {
            get => _searchText;
            set { SetField(ref _searchText, value); ApplyFilterAndSort(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { SetField(ref _isLoading, value); _setLoading(value); }
        }

        public bool SortAscending
        {
            get => _sortAscending;
            set { SetField(ref _sortAscending, value); OnPropertyChanged(nameof(SortLabel)); }
        }

        public string SortLabel => SortAscending ? "Date ↑" : "Date ↓";

        public int TotalCount
        {
            get => _totalCount;
            private set => SetField(ref _totalCount, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand ToggleSortCommand { get; }

        public AllRecordsViewModel(IExcelService excelService, Action<bool> setLoading)
        {
            _excelService = excelService;
            _setLoading = setLoading;
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);
            ToggleSortCommand = new RelayCommand(() =>
            {
                SortAscending = !SortAscending;
                ApplyFilterAndSort();
            });
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                _allRecords = await _excelService.LoadRecordsAsync();
                TotalCount = _allRecords.Count;
                ApplyFilterAndSort();
            }
            catch (Exception ex)
            {
                NotificationService.Instance.Show("Error", $"Failed to load data: {ex.Message}", NotificationType.Error);
            }
            finally { IsLoading = false; }
        }

        private void ApplyFilterAndSort()
        {
            IEnumerable<IncidentRecord> filtered = _allRecords;

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(r =>
                    r.SubjectLine.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Incident.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.ShortDescription.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.CreatedBy.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Status.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            filtered = SortAscending
                ? filtered.OrderBy(r => r.Date).ThenBy(r => r.RowIndex)
                : filtered.OrderByDescending(r => r.Date).ThenByDescending(r => r.RowIndex);

            DisplayedRecords = new ObservableCollection<IncidentRecord>(filtered);
        }
    }
}
