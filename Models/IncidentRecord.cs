using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IncidentTracker.Models
{
    public class IncidentRecord : INotifyPropertyChanged
    {
        private int _rowIndex;
        private DateTime _date;
        private string _createdBy = "";
        private string _subjectLine = "";
        private string _incident = "";
        private string _status = "InProgress";
        private string _shortDescription = "";

        public int RowIndex
        {
            get => _rowIndex;
            set { _rowIndex = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public string CreatedBy
        {
            get => _createdBy;
            set { _createdBy = value ?? ""; OnPropertyChanged(); }
        }

        public string SubjectLine
        {
            get => _subjectLine;
            set { _subjectLine = value ?? ""; OnPropertyChanged(); }
        }

        public string Incident
        {
            get => _incident;
            set { _incident = value ?? ""; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value ?? "InProgress"; OnPropertyChanged(); }
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set { _shortDescription = value ?? ""; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public IncidentRecord Clone() => new()
        {
            RowIndex = RowIndex,
            Date = Date,
            CreatedBy = CreatedBy,
            SubjectLine = SubjectLine,
            Incident = Incident,
            Status = Status,
            ShortDescription = ShortDescription
        };
    }
}
