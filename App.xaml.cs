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

            // Use the absolute target path as the unified local database
            string excelPath = @"C:\Projects\Incident Tracker\XYZ.xlsx";
            ExcelService = new ExcelService(excelPath);
        }
    }
}
