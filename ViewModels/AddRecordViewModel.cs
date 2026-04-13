using System.Windows.Input;
using IncidentTracker.Commands;
using IncidentTracker.Models;
using IncidentTracker.Services;

namespace IncidentTracker.ViewModels
{
    public class AddRecordViewModel : BaseViewModel
    {
        private readonly IExcelService _excelService;
        private DateTime _date = DateTime.Now;
        private string _createdBy = "";
        private string _subjectLine = "";
        private string _incident = "";
        private string _status = "InProgress";
        private string _shortDescription = "";
        private bool _isLoading;

        public DateTime Date { get => _date; set => SetField(ref _date, value); }
        public string CreatedBy { get => _createdBy; set => SetField(ref _createdBy, value); }
        public string SubjectLine { get => _subjectLine; set => SetField(ref _subjectLine, value); }
        public string Incident { get => _incident; set => SetField(ref _incident, value); }
        public string Status { get => _status; set => SetField(ref _status, value); }
        public string ShortDescription { get => _shortDescription; set => SetField(ref _shortDescription, value); }
        public bool IsLoading { get => _isLoading; set => SetField(ref _isLoading, value); }

        public List<string> StatusOptions { get; } = new() { "InProgress", "Completed" };

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? SubmitSucceeded;
        public event Action? CancelRequested;

        public AddRecordViewModel(IExcelService excelService)
        {
            _excelService = excelService;
            CreatedBy = OutlookService.GetCurrentUserName();
            SubmitCommand = new AsyncRelayCommand(SubmitAsync);
            CancelCommand = new RelayCommand(() => CancelRequested?.Invoke());
        }

        private async Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(SubjectLine))
            {
                NotificationService.Instance.Show("Validation", "Subject Line is required.", NotificationType.Warning);
                return;
            }

            IsLoading = true;
            try
            {
                var record = new IncidentRecord
                {
                    Date = Date,
                    CreatedBy = CreatedBy,
                    SubjectLine = SubjectLine,
                    Incident = Incident,
                    Status = Status,
                    ShortDescription = ShortDescription
                };

                await _excelService.AddRecordAsync(record);
                NotificationService.Instance.Show("Success", "Record added successfully.", NotificationType.Success);
                SubmitSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                NotificationService.Instance.Show("Error", $"Failed to add record: {ex.Message}", NotificationType.Error);
            }
            finally { IsLoading = false; }
        }
    }
}
