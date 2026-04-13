using System.Windows;
using IncidentTracker.Services;

namespace IncidentTracker
{
    public partial class App : Application
    {
        public static IExcelService ExcelService { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName 
                             ?? AppDomain.CurrentDomain.BaseDirectory;
            string dir = System.IO.Path.GetDirectoryName(exePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            string excelPath = System.IO.Path.Combine(dir, "XYZ.xlsx");
            ExcelService = new ExcelService(excelPath);
        }
    }
}
