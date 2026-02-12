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
                page.Size(12, 8, Unit.Centimetre);
                page.Margin(0.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Absolute Cinema Ticket")
                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(0.5f, Unit.Centimetre)
                    .Row(r =>
                    {
                        r.RelativeItem().Column(column =>
                        {
                            column.Item().Text($"Фільм: {movieName}").FontSize(14).Bold();
                            column.Item().Text($"Дата: {date:dd.MM.yyyy HH:mm}").FontSize(12);
                            column.Item().Text($"Зал: {hallName}");

                            column.Item().PaddingTop(5).Text($"Ряд: {row} | Місце: {seat}")
                                .FontSize(13).Bold().FontColor(Colors.Red.Medium);
                        });

                        r.ConstantItem(80).Image(qrCodeBytes);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("ID: ").FontSize(8);
                        x.Span(ticketId.ToString()).FontSize(8).Italic();
                    });
            });
        });

        return document.GeneratePdf();
    }
}