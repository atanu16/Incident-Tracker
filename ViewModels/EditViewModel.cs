using System.Collections.ObjectModel;
using System.Windows.Input;
using IncidentTracker.Commands;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.ViewModels
{
    public class EditViewModel : BaseViewModel
    {
        private readonly IExcelService _excelService;
        private readonly Action<bool> _setLoading;
        private bool _isLoading;
        private string _searchText = "";
        private List<IncidentRecord> _allRecords = new();
        private ObservableCollection<IncidentRecord> _records = new();

        public ObservableCollection<IncidentRecord> Records
        {
            get => _records;
            set => SetField(ref _records, value);
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

        public List<string> StatusOptions { get; } = new() { "InProgress", "Completed" };

        public ICommand SaveCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand DeleteRecordCommand { get; }

        public EditViewModel(IExcelService excelService, Action<bool> setLoading)
        {
            _excelService = excelService;
            _setLoading = setLoading;
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);
            ClearSearchCommand = new RelayCommand(() => SearchText = "");
            DeleteRecordCommand = new RelayCommand(param =>
            {
                if (param is IncidentRecord record)
                {
                    _allRecords.Remove(record);
                    Records.Remove(record);
                }
            });
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                _allRecords = await _excelService.LoadRecordsAsync();
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
            IEnumerable<IncidentRecord> filtered = _allRecords;

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(r =>
                    r.SubjectLine.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Incident.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.ShortDescription.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.CreatedBy.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    r.Status.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            Records = new ObservableCollection<IncidentRecord>(filtered.OrderByDescending(r => r.RowIndex));
        }

        private async Task SaveAsync()
        {
            IsLoading = true;
            try
            {
                await _excelService.UpdateRecordsAsync(_allRecords);
                NotificationService.Instance.Show("Success", "All records saved successfully.", NotificationType.Success);
            }
            catch (Exception ex)
            {
                NotificationService.Instance.Show("Error", $"Save failed: {ex.Message}", NotificationType.Error);
            }
            finally { IsLoading = false; }
        }
    }
}
