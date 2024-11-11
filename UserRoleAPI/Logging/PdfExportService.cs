using UserRoleAPI.DataAccessLayer.Models;
using UserRoleAPI.Logging.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UserRoleAPI.Logging
{
    public class ExportPdfService : IExportPdfService
    {
        private const int PageLimit = 3;

        public async Task<byte[]> ExportUsersToPdfAsync(IEnumerable<User> users)
        {
            using var memoryStream = new MemoryStream();
            using var document = new Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Create table with columns
            var table = new PdfPTable(2) { WidthPercentage = 100 };

            // Date in a single cell spanning two columns
            var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var dateCell = new PdfPCell(new Phrase($"Date: {DateTime.Now:yyyy-MM-dd}", dateFont))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5,
                BackgroundColor = new BaseColor(240, 240, 240) // Light grey background for date
            };
            table.AddCell(dateCell);

            // Title in a single cell spanning two columns
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var titleCell = new PdfPCell(new Phrase("Users List", titleFont))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 8,
                BackgroundColor = new BaseColor(200, 200, 200) // Slightly darker grey for title
            };
            table.AddCell(titleCell);

            // Header row
            table.AddCell(CreateHeaderCell("User Name"));
            table.AddCell(CreateHeaderCell("Email"));

            // Populate data and add page breaks after PageLimit records
            int counter = 0;
            foreach (var user in users)
            {
                table.AddCell(CreateCenteredCell(user.UserName));
                table.AddCell(CreateCenteredCell(user.Email));
                counter++;

                // Add a page break after the specified number of records (PageLimit)
                if (counter % PageLimit == 0)
                {
                    document.Add(table);
                    document.NewPage();
                    table = new PdfPTable(2) { WidthPercentage = 100 };

                    // Add headers again on new page
                    table.AddCell(CreateHeaderCell("User Name"));
                    table.AddCell(CreateHeaderCell("Email"));
                }
            }

            // If there are any remaining rows to be added after the last page break
            if (counter % PageLimit != 0)
            {
                document.Add(table);
            }

            document.Close();

            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportRolesToPdfAsync(IEnumerable<Role> roles)
        {
            using var memoryStream = new MemoryStream();
            using var document = new Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Create table with columns
            var table = new PdfPTable(2) { WidthPercentage = 100 };

            // Date in a single cell spanning two columns
            var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var dateCell = new PdfPCell(new Phrase($"Date: {DateTime.Now:yyyy-MM-dd}", dateFont))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5,
                BackgroundColor = new BaseColor(240, 240, 240) // Light grey background for date
            };
            table.AddCell(dateCell);

            // Title in a single cell spanning two columns
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var titleCell = new PdfPCell(new Phrase("Roles List", titleFont))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 8,
                BackgroundColor = new BaseColor(200, 200, 200) // Slightly darker grey for title
            };
            table.AddCell(titleCell);

            // Header row
            table.AddCell(CreateHeaderCell("Role Name"));
            table.AddCell(CreateHeaderCell("Created Date"));

            // Populate data and add page breaks after PageLimit records
            int counter = 0;
            foreach (var role in roles)
            {
                table.AddCell(CreateCenteredCell(role.RoleName));
                table.AddCell(CreateCenteredCell(role.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")));
                counter++;

                // Add a page break after the specified number of records (PageLimit)
                if (counter % PageLimit == 0)
                {
                    document.Add(table);
                    document.NewPage();
                    table = new PdfPTable(2) { WidthPercentage = 100 };

                    // Add headers again on new page
                    table.AddCell(CreateHeaderCell("Role Name"));
                    table.AddCell(CreateHeaderCell("Created Date"));
                }
            }

            // If there are any remaining rows to be added after the last page break
            if (counter % PageLimit != 0)
            {
                document.Add(table);
            }

            document.Close();

            return memoryStream.ToArray();
        }

        private PdfPCell CreateHeaderCell(string text)
        {
            var cell = new PdfPCell(new Phrase(text, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
            {
                BackgroundColor = new BaseColor(192, 192, 192), // Light grey background for headers
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5
            };
            return cell;
        }

        private PdfPCell CreateCenteredCell(string text)
        {
            var cell = new PdfPCell(new Phrase(text))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5
            };
            return cell;
        }
    }
}
