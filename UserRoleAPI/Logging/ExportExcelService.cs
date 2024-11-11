using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UserRoleAPI.DataAccessLayer.Models;
using UserRoleAPI.Logging.Interfaces;

namespace UserRoleAPI.Logging
{
    public class ExportExcelService : IExportExcelService
    {
        private const int PageLimit = 25;

        public async Task<byte[]> ExportUsersToExcelAsync(IEnumerable<User> users)
        {
            using var workbook = new XSSFWorkbook();
            var sheet = CreateSheetWithHeader(workbook, "Users", true);

            int rowIndex = 3;
            int userCount = 0;
            foreach (var user in users)
            {
                if (userCount >= PageLimit)
                {
                    sheet = CreateSheetWithHeader(workbook, $"Users {workbook.NumberOfSheets + 1}", false);
                    rowIndex = 0;
                    userCount = 0;
                }

                var row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(user.UserName);
                row.CreateCell(1).SetCellValue(user.Email);

                // Center align cell values
                for (int i = 0; i <= 1; i++)
                {
                    row.GetCell(i).CellStyle = CreateDataStyle(workbook);
                }

                userCount++;
            }

            // Write to stream
            using var stream = new MemoryStream();
            workbook.Write(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> ExportRolesToExcelAsync(IEnumerable<Role> roles)
        {
            using var workbook = new XSSFWorkbook();
            var sheet = CreateSheetWithHeader(workbook, "Roles", true);

            int rowIndex = 3;
            int roleCount = 0;
            foreach (var role in roles)
            {
                if (roleCount >= PageLimit)
                {
                    sheet = CreateSheetWithHeader(workbook, $"Roles {workbook.NumberOfSheets + 1}", false);
                    rowIndex = 0;
                    roleCount = 0;
                }

                var row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(role.RoleName);
                row.CreateCell(1).SetCellValue(role.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"));

                // Center align cell values
                for (int i = 0; i <= 1; i++)
                {
                    row.GetCell(i).CellStyle = CreateDataStyle(workbook);
                }

                roleCount++;
            }

            // Write to stream
            using var stream = new MemoryStream();
            workbook.Write(stream);
            return stream.ToArray();
        }

        private ISheet CreateSheetWithHeader(IWorkbook workbook, string sheetName, bool includeTitleAndDate)
        {
            var sheet = workbook.CreateSheet(sheetName);

            if (includeTitleAndDate)
            {
                // Add generated date row
                var dateRow = sheet.CreateRow(0);
                dateRow.HeightInPoints = 20;
                var dateCell = dateRow.CreateCell(0);
                dateCell.SetCellValue($"Date: {DateTime.Now:yyyy-MM-dd}");
                dateCell.CellStyle = CreateDateStyle(workbook);

                // Merge cells A1 and B1 for the date
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 1));

                // Add title row
                var titleRow = sheet.CreateRow(1);
                titleRow.HeightInPoints = 25;
                var titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue($"{sheetName} List");
                titleCell.CellStyle = CreateTitleStyle(workbook);

                // Merge cells A2 and B2 for the title
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 1));
            }

            // Create header row (third row, if title and date are included, else row 0)
            var headerRow = sheet.CreateRow(includeTitleAndDate ? 2 : 0);
            headerRow.HeightInPoints = 20;
            headerRow.CreateCell(0).SetCellValue(sheetName == "Users" ? "User Name" : "Role Name");
            headerRow.CreateCell(1).SetCellValue(sheetName == "Users" ? "Email" : "Created Date");

            // Apply header style
            for (int i = 0; i <= 1; i++)
            {
                headerRow.GetCell(i).CellStyle = CreateHeaderStyle(workbook);
            }

            // Manually set column widths to increase cell size (approx. 20-30 characters wide)
            sheet.SetColumnWidth(0, 7000);
            sheet.SetColumnWidth(1, 7000);

            return sheet;
        }

        private ICellStyle CreateDateStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 12;
            font.IsBold = true;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Right;

            // Set light grey background using IndexedColors
            style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;

            return ApplyBorderStyle(style);
        }

        private ICellStyle CreateTitleStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 16;
            font.IsBold = true;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            // Set slightly darker grey background
            style.FillForegroundColor = IndexedColors.Grey40Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;

            return ApplyBorderStyle(style);
        }

        private ICellStyle CreateHeaderStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.Alignment = HorizontalAlignment.Center;

            return ApplyBorderStyle(style);
        }

        private ICellStyle CreateDataStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Center;

            return ApplyBorderStyle(style);
        }

        private ICellStyle ApplyBorderStyle(ICellStyle style)
        {
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            return style;
        }
    }
}
