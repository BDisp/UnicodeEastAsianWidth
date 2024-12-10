namespace UnicodeEastAsianWidth.Generator;

public class UnicodeEastAsianWidthParser
{
    public static List<(string Start, string End, string Width, string GeneralCategory, int RangeLength, string StartName, string EndName)> ParseFile(string filePath)
    {
        var entries = new List<(string Start, string End, string Width, string GeneralCategory, int RangeLength, string StartName, string EndName)>();

        foreach (var line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            // Split line into parts
            var parts = line.Split(';', '#', '[', ']');
            var range = parts[0].Trim();
            var width = parts[1].Trim();
            var generalCategory = parts.Length == 3 ? parts[2].Substring(0, 3).Trim() : parts[2].Trim();
            var rangeLength = parts.Length == 5 ? int.Parse(parts[3]) : 1;
            var nameRange = parts.Length == 3 ? parts[2].Substring(3).Trim() : parts[^1];

            // Parse range
            var rangeParts = range.Split("..");
            var start = rangeParts[0].Trim();
            var end = rangeParts.Length > 1 ? rangeParts[1].Trim() : start;
            var nameRangeParts = nameRange.Split("..");
            var startName = nameRangeParts[0].Trim();
            var endName = nameRangeParts.Length > 1 ? nameRangeParts[1].Trim() : startName;

            // Add to entries list
            entries.Add((start, end, width, generalCategory, rangeLength, startName, endName));
        }

        return entries;
    }
}