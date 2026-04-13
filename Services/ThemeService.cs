using System.Windows;

namespace IncidentTracker.Services
{
    public static class ThemeService
    {
        public static void ApplyTheme(bool isDark)
        {
            var app = Application.Current;
            if (app == null) return;

            var dicts = app.Resources.MergedDictionaries;
            var toRemove = dicts
                .Where(d => d.Source?.OriginalString?.Contains("Theme.xaml") == true)
                .ToList();
            foreach (var d in toRemove) dicts.Remove(d);

            var themeName = isDark ? "DarkTheme" : "LightTheme";
            dicts.Add(new ResourceDictionary
            {
                Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative)
            });
        }
    }
}
