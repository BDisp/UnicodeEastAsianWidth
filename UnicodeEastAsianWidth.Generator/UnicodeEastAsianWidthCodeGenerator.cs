using System.Text;

namespace UnicodeEastAsianWidth.Generator;

public static class UnicodeEastAsianWidthCodeGenerator
{
    public static string GenerateCode(List<(string Start, string End, string Width, string GeneralCategory, int RangeLength, string StartName, string EndName)> entries)
    {
        var sb = new StringBuilder();

        // Add namespace and enum definition
        sb.AppendLine("namespace UnicodeEastAsianWidth");
        sb.AppendLine("{");
        sb.AppendLine("    public enum EastAsianWidth { Neutral, Narrow, Wide, Ambiguous, HalfWidth, FullWidth }");
        sb.AppendLine();
        sb.AppendLine("    public static class EastAsianWidthData");
        sb.AppendLine("    {");
        sb.AppendLine("        public record Entry(uint Start, uint End, EastAsianWidth Width, string GeneralCategory, int RangeLength, string StartName, string EndName);");
        sb.AppendLine();
        sb.AppendLine("        public static readonly Entry[] Entries = new[]");
        sb.AppendLine("        {");

        foreach (var entry in entries)
        {
            // Cleanly extract the needed values
            var width = entry.Width switch
            {
                "A" => "Ambiguous",
                "F" => "FullWidth",
                "H" => "HalfWidth",
                "N" => "Neutral",
                "Na" => "Narrow",
                "W" => "Wide",
                _ => throw new ArgumentOutOfRangeException()
            };

            // Format the entry properly
            sb.AppendLine($"            new Entry(0x{entry.Start}, 0x{entry.End}, EastAsianWidth.{width}, \"{entry.GeneralCategory}\", {entry.RangeLength}, \"{entry.StartName}\", \"{entry.EndName}\"),");
        }

        sb.AppendLine("        };");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
