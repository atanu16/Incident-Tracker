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
        private ObservableCollection<IncidentRecord> _records = new();

        public ObservableCollection<IncidentRecord> Records
        {
            get => _records;
            set => SetField(ref _records, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { SetField(ref _isLoading, value); _setLoading(value); }
        }

        public List<string> StatusOptions { get; } = new() { "InProgress", "Completed" };

        public ICommand SaveCommand { get; }
        public ICommand RefreshCommand { get; }

        public EditViewModel(IExcelService excelService, Action<bool> setLoading)
        {
            _excelService = excelService;
            _setLoading = setLoading;
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var records = await _excelService.LoadRecordsAsync();
                Records = new ObservableCollection<IncidentRecord>(records);
            }
            catch (Exception ex)
            {
                NotificationService.Instance.Show("Error", $"Failed to load data: {ex.Message}", NotificationType.Error);
            }
            finally { IsLoading = false; }
        }

        private async Task SaveAsync()
        {
            IsLoading = true;
            try
            {
                await _excelService.UpdateRecordsAsync(Records);
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
