using System.Collections.ObjectModel;
using System.Windows;
using IncidentTracker.Models;

namespace IncidentTracker.Services
{
    public class NotificationService
    {
        private static NotificationService? _instance;
        public static NotificationService Instance => _instance ??= new NotificationService();

        public ObservableCollection<NotificationItem> Notifications { get; } = new();

        private NotificationService() { }

        public void Show(string title, string message, NotificationType type = NotificationType.Info, int durationMs = 3000)
        {
            var item = new NotificationItem
            {
                Title = title,
                Message = message,
                Type = type,
                DurationMs = durationMs
            };

            Application.Current?.Dispatcher.Invoke(() => Notifications.Add(item));
        }

        public void Remove(NotificationItem item)
        {
            Application.Current?.Dispatcher.Invoke(() => Notifications.Remove(item));
        }
    }
}
