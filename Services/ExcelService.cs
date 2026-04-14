using System.IO;
using ClosedXML.Excel;
using IncidentTracker.Models;

namespace IncidentTracker.Services
{
    public interface IExcelService
    {
        Task<List<IncidentRecord>> LoadRecordsAsync();
        Task AddRecordAsync(IncidentRecord record);
        Task UpdateRecordsAsync(IEnumerable<IncidentRecord> records);
    }

    public class ExcelService : IExcelService
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public ExcelService(string filePath) => _filePath = filePath;

        public async Task<List<IncidentRecord>> LoadRecordsAsync()
        {
            await _lock.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    var records = new List<IncidentRecord>();
                    if (!File.Exists(_filePath))
                    {
                        CreateNewFile();
                        return records;
                    }

                    using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var wb = new XLWorkbook(fs);
                    var ws = wb.Worksheet(1);
                    var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

                    for (int row = 2; row <= lastRow; row++)
                    {
                        var r = ws.Row(row);
                        var cell1 = r.Cell(1).GetString();
                        var cell3 = r.Cell(3).GetString();
                        if (string.IsNullOrWhiteSpace(cell1) && string.IsNullOrWhiteSpace(cell3))
                            continue;

                        var statusRaw = r.Cell(5).GetString();
                        records.Add(new IncidentRecord
                        {
                            RowIndex = row,
                            Date = ParseDate(cell1),
                            CreatedBy = r.Cell(2).GetString(),
                            SubjectLine = cell3,
                            Incident = r.Cell(4).GetString(),
                            Status = string.IsNullOrWhiteSpace(statusRaw) ? "InProgress" : statusRaw,
                            ShortDescription = r.Cell(6).GetString()
                        });
                    }
                    return records;
                });
            }
            finally { _lock.Release(); }
        }

        public async Task AddRecordAsync(IncidentRecord record)
        {
            await _lock.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    if (!File.Exists(_filePath)) CreateNewFile();

                    using var wb = new XLWorkbook(_filePath);
                    var ws = wb.Worksheet(1);
                    int newRow = (ws.LastRowUsed()?.RowNumber() ?? 1) + 1;

                    ws.Cell(newRow, 1).Value = record.Date.ToString("yyyy-MM-dd");
                    ws.Cell(newRow, 2).Value = record.CreatedBy;
                    ws.Cell(newRow, 3).Value = record.SubjectLine;
                    ws.Cell(newRow, 4).Value = record.Incident;
                    ws.Cell(newRow, 5).Value = record.Status;
                    ws.Cell(newRow, 6).Value = record.ShortDescription;

                    StyleDataRow(ws, newRow);
                    wb.Save();
                });
            }
            finally { _lock.Release(); }
        }

        public async Task UpdateRecordsAsync(IEnumerable<IncidentRecord> records)
        {
            await _lock.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    if (!File.Exists(_filePath)) return;

                    using var wb = new XLWorkbook(_filePath);
                    var ws = wb.Worksheet(1);

                    foreach (var record in records)
                    {
                        if (record.RowIndex < 2) continue;
                        ws.Cell(record.RowIndex, 1).Value = record.Date.ToString("yyyy-MM-dd");
                        ws.Cell(record.RowIndex, 2).Value = record.CreatedBy;
                        ws.Cell(record.RowIndex, 3).Value = record.SubjectLine;
                        ws.Cell(record.RowIndex, 4).Value = record.Incident;
                        ws.Cell(record.RowIndex, 5).Value = record.Status;
                        ws.Cell(record.RowIndex, 6).Value = record.ShortDescription;
                    }
                    wb.Save();
                });
            }
            finally { _lock.Release(); }
        }

        private void CreateNewFile()
        {
            var dir = System.IO.Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir)) System.IO.Directory.CreateDirectory(dir);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Incidents");

            string[] headers = { "Date", "Created By", "Subject Line", "Incident", "Status", "Short Description" };
            for (int i = 0; i < headers.Length; i++)
                ws.Cell(1, i + 1).Value = headers[i];

            var headerRow = ws.Range(1, 1, 1, 6);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.FontSize = 11;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#4A3FA0");
            headerRow.Style.Font.FontColor = XLColor.White;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Column(1).Width = 18;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 30;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 40;

            wb.SaveAs(_filePath);
        }

        private static void StyleDataRow(IXLWorksheet ws, int row)
        {
            var range = ws.Range(row, 1, row, 6);
            range.Style.Font.FontSize = 10;
            range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        private static DateTime ParseDate(string s)
        {
            if (DateTime.TryParse(s, out var d)) return d;
            return DateTime.Now;
        }
    }
}
