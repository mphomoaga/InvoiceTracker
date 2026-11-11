using InvoiceTracker.Domain.Interfaces;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace InvoiceTracker.Infrastructure.PDFGenerator
{
    public class PdfGenerator : IPdfGenerator
    {
        public Task<byte[]> GeneratePdfFromHTML(string html)
        {
            ArgumentNullException.ThrowIfNull(html);

            var text = Regex.Replace(
                html,
                @"<\s*(br|/p|/div|/h[1-6]|/tr|/li)\b[^>]*>",
                "\n",
                RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<[^>]+>", " ");
            text = WebUtility.HtmlDecode(text).Replace('\r', ' ');

            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(WrapLine)
                .Take(50)
                .ToArray();

            return Task.FromResult(CreatePdf(lines.Length == 0 ? ["Invoice"] : lines));
        }

        private static IEnumerable<string> WrapLine(string value)
        {
            var remaining = Regex.Replace(value, @"\s+", " ").Trim();
            while (remaining.Length > 90)
            {
                var breakAt = remaining.LastIndexOf(' ', 90);
                if (breakAt <= 0)
                {
                    breakAt = 90;
                }

                yield return remaining[..breakAt];
                remaining = remaining[breakAt..].TrimStart();
            }

            if (remaining.Length > 0)
            {
                yield return remaining;
            }
        }

        private static byte[] CreatePdf(IEnumerable<string> lines)
        {
            var content = new StringBuilder("BT\n/F1 11 Tf\n50 750 Td\n14 TL\n");
            foreach (var line in lines)
            {
                content.Append('(')
                    .Append(EscapePdfText(line))
                    .Append(") Tj\nT*\n");
            }

            content.Append("ET\n");
            var contentBytes = Encoding.ASCII.GetBytes(content.ToString());
            var objects = new[]
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
                $"<< /Length {contentBytes.Length} >>\nstream\n{Encoding.ASCII.GetString(contentBytes)}endstream"
            };

            using var output = new MemoryStream();
            WriteAscii(output, "%PDF-1.4\n");
            var offsets = new List<long> { 0 };
            for (var index = 0; index < objects.Length; index++)
            {
                offsets.Add(output.Position);
                WriteAscii(output, $"{index + 1} 0 obj\n{objects[index]}\nendobj\n");
            }

            var crossReferenceOffset = output.Position;
            WriteAscii(output, $"xref\n0 {offsets.Count}\n0000000000 65535 f \n");
            foreach (var offset in offsets.Skip(1))
            {
                WriteAscii(output, $"{offset:0000000000} 00000 n \n");
            }

            WriteAscii(output, $"trailer\n<< /Size {offsets.Count} /Root 1 0 R >>\nstartxref\n{crossReferenceOffset}\n%%EOF");
            return output.ToArray();
        }

        private static string EscapePdfText(string value)
        {
            var ascii = new string(value.Select(character => character is >= ' ' and <= '~' ? character : '?').ToArray());
            return ascii.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private static void WriteAscii(Stream output, string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            output.Write(bytes, 0, bytes.Length);
        }
    }
}
