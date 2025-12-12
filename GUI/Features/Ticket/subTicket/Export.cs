using DTO.Ticket;
using DTO.Ticket.DTO.Ticket;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace GUI.Export
{
    public static class TicketPdfExporter
    {
        public static void Export(TicketDetailDTO dto, string filePath)
        {
            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            using var doc = new iText.Layout.Document(pdf);

            // ===== FONTS =====
            PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont fontItalic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            // ===== TITLE =====
            doc.Add(
                new Paragraph("AIRLINE TICKET")
                    .SetFont(fontBold)
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
            );

            doc.Add(
                new Paragraph($"Ticket No: {dto.TicketNumber}")
                    .SetTextAlignment(TextAlignment.CENTER)
            );

            doc.Add(new Paragraph(" "));

            // ===== INFO TABLE =====
            var table = new iText.Layout.Element.Table(2)
                .UseAllAvailableWidth();

            AddRow(table, "Passenger", dto.PassengerName, fontBold, fontNormal);
            AddRow(table, "Passport", dto.PassportNumber, fontBold, fontNormal);
            AddRow(table, "Nationality", dto.Nationality, fontBold, fontNormal);

            AddRow(table, "Flight", dto.FlightNumber, fontBold, fontNormal);
            AddRow(table, "Route", dto.Route, fontBold, fontNormal);
            AddRow(table, "Departure", dto.DepartureTime.ToString("dd/MM/yyyy HH:mm"), fontBold, fontNormal);
            AddRow(table, "Arrival", dto.ArrivalTime.ToString("dd/MM/yyyy HH:mm"), fontBold, fontNormal);

            AddRow(table, "Seat", dto.SeatNumber, fontBold, fontNormal);
            AddRow(table, "Cabin", dto.CabinClass, fontBold, fontNormal);
            AddRow(table, "Price", dto.TotalPrice.ToString("N0"), fontBold, fontNormal);
            AddRow(table, "Status", dto.Status, fontBold, fontNormal);

            doc.Add(table);

            // ===== FOOTER =====
            doc.Add(
                new Paragraph("This is a system-generated ticket for reference only.")
                    .SetFont(fontItalic)
                    .SetFontSize(9)
            );
        }

        private static void AddRow(
            iText.Layout.Element.Table table,
            string label,
            string value,
            PdfFont fontLabel,
            PdfFont fontValue)
        {
            table.AddCell(
                new Cell().Add(
                    new Paragraph(label).SetFont(fontLabel)
                )
            );

            table.AddCell(
                new Cell().Add(
                    new Paragraph(value ?? "—").SetFont(fontValue)
                )
            );
        }
    }
}
