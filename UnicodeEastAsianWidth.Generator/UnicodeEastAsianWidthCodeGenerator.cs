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
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Represents the East Asian Width categories for Unicode characters.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <remarks>");
        sb.AppendLine("    /// The East Asian Width property is used to classify Unicode characters based on their width ");
        sb.AppendLine("    /// in contexts such as terminal emulators or text rendering. The categories include:");
        sb.AppendLine("    /// For more information, refer to the Unicode Consortium's East Asian Width specification.");
        sb.AppendLine("    /// </remarks>");
        sb.AppendLine("    public enum EastAsianWidth");
        sb.AppendLine("    {");
        sb.AppendLine("        /// Characters with no specific width classification.");
        sb.AppendLine("        Neutral,");
        sb.AppendLine("        /// Characters that typically occupy a narrow cell.");
        sb.AppendLine("        Narrow,");
        sb.AppendLine("        /// Characters that typically occupy a wide cell.");
        sb.AppendLine("        Wide,");
        sb.AppendLine("        /// Characters whose width can vary depending on the context.");
        sb.AppendLine("        Ambiguous,");
        sb.AppendLine("        /// Characters that occupy half the width of a standard wide cell.");
        sb.AppendLine("        HalfWidth,");
        sb.AppendLine("        /// Characters that occupy the full width of a standard wide cell.");
        sb.AppendLine("        FullWidth");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Provides access to East Asian Width data for Unicode characters.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <remarks>");
        sb.AppendLine("    /// The <c>EastAsianWidthData</c> class contains a static list of <see cref=\"Entry\" />");
        sb.AppendLine("    /// objects, representing Unicode character ranges and their metadata.");
        sb.AppendLine("    /// This data is derived from the Unicode Consortium's East Asian Width specification.");
        sb.AppendLine("    /// </remarks>");
        sb.AppendLine("    /// <example>");
        sb.AppendLine("    /// Example usage:");
        sb.AppendLine("    /// <code>");
        sb.AppendLine("    /// var width = EastAsianWidthData.Entries.FirstOrDefault(entry => codePoint &amp;gt;= entry.Start &amp;&amp; codePoint &amp;lt;= entry.End)?.Width;");
        sb.AppendLine("    /// </code>");
        sb.AppendLine("    /// </example>");
        sb.AppendLine("    public static class EastAsianWidthData");
        sb.AppendLine("    {");
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Represents a range of Unicode code points and their associated East Asian Width metadata.");
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        /// <param name=\"Start\">The starting Unicode code point of the range.</param>");
        sb.AppendLine("        /// <param name=\"End\">The ending Unicode code point of the range.</param>");
        sb.AppendLine("        /// <param name=\"Width\">The East Asian Width classification for the range.</param>");
        sb.AppendLine("        /// <param name=\"GeneralCategory\">The general Unicode category (e.g., \"Lo\" for Letter, Other).</param>");
        sb.AppendLine("        /// <param name=\"RangeLength\">The number of code points in the range.</param>");
        sb.AppendLine("        /// <param name=\"StartName\">The descriptive name of the starting code point.</param>");
        sb.AppendLine("        /// <param name=\"EndName\">The descriptive name of the ending code point.</param>");
        sb.AppendLine("        /// <remarks>");
        sb.AppendLine("        /// This record provides detailed information about a contiguous range of Unicode characters. ");
        sb.AppendLine("        /// It includes the range's start and end points, the East Asian Width classification, and ");
        sb.AppendLine("        /// additional metadata such as the Unicode general category and names for the start and end points.");
        sb.AppendLine("        /// </remarks>");
        sb.AppendLine("        /// <example>");
        sb.AppendLine("        /// Example usage:");
        sb.AppendLine("        /// <code>");
        sb.AppendLine("        /// var entry = new Entry(0x4E00, 0x4E2F, EastAsianWidth.Wide, \"Lo\", 48, \"CJK Ideograph 一\", \"CJK Ideograph 丯\");");
        sb.AppendLine("        /// </code>");
        sb.AppendLine("        /// </example>");
        sb.AppendLine("        public record Entry(uint Start, uint End, EastAsianWidth Width, string GeneralCategory, int RangeLength, string StartName, string EndName);");
        sb.AppendLine();
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// A collection of all Unicode character ranges with their associated East Asian Width metadata.");
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        /// <remarks>");
        sb.AppendLine("        /// This property provides a comprehensive list of Unicode character ranges. Each entry includes");
        sb.AppendLine("        /// the starting and ending code points, the East Asian Width classification, general Unicode");
        sb.AppendLine("        /// category, range length, and the descriptive names of the start and end points.");
        sb.AppendLine("        /// </remarks>");
        sb.AppendLine("        public static readonly Entry[] Entries =");
        sb.AppendLine("        [");

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

        sb.AppendLine("        ];");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}
