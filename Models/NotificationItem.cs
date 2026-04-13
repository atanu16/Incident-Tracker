using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IncidentTracker.Models
{
    public enum NotificationType { Success, Error, Warning, Info }

    public class NotificationItem : INotifyPropertyChanged
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public NotificationType Type { get; set; } = NotificationType.Info;
        public int DurationMs { get; set; } = 3000;

        public string Icon => Type switch
        {
            NotificationType.Success => "✓",
            NotificationType.Error => "✕",
            NotificationType.Warning => "⚠",
            _ => "ℹ"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
