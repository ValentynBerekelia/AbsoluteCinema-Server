using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;

public class PdfTicketGenerator
{
    public static byte[] GenerateTicket(string movieName, string hallName, int row, int seat, DateTime date, Guid ticketId)
    {
        // QR
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(ticketId.ToString(), QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20);

        // PDF
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5.Landscape());
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Absolute Cinema Ticket")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text($"Фільм: {movieName}").FontSize(16).Bold();
                            column.Item().Text($"Дата: {date:dd.MM.yyyy HH:mm}").FontSize(14);
                            column.Item().Text($"Зал: {hallName}");
                            column.Item().Text($"Ряд: {row} | Місце: {seat}").FontSize(14).Bold().FontColor(Colors.Red.Medium);
                        });

                        row.ConstantItem(100).Image(qrCodeBytes);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("ID квитка: ");
                        x.Span(ticketId.ToString()).Italic();
                    });
            });
        });

        return document.GeneratePdf();
    }
}